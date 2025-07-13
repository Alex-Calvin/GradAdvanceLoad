using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Polly;
using DataProcessor.Configuration;
using DataProcessor.Models;
using DataProcessor.Services;

namespace DataProcessor.Services
{
    public interface IDataProcessorService
    {
        Task ProcessUnprocessedRecordsAsync();
    }

    public class DataProcessorService : IDataProcessorService
    {
        private readonly DataContext _dataContext;
        private readonly IApiService _apiService;
        private readonly RetrySettings _retrySettings;

        public DataProcessorService(DataContext dataContext, IApiService apiService, RetrySettings retrySettings)
        {
            _dataContext = dataContext;
            _apiService = apiService;
            _retrySettings = retrySettings;
        }

        public async Task ProcessUnprocessedRecordsAsync()
        {
            var unprocessedRecords = await _dataContext.UserRecords
                .Where(x => x.ProcessedIndicator == null)
                .ToListAsync();

            if (!unprocessedRecords.Any())
            {
                Console.WriteLine("No unprocessed records found.");
                return;
            }

            var random = new Random();
            var retryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryForeverAsync(
                    retryAttempt =>
                    {
                        var calculatedDelayMs = Math.Pow(2, retryAttempt) * _retrySettings.BaseDelayMs;
                        var jitterMs = random.Next(0, _retrySettings.JitterMaxMs);
                        var actualDelay = Math.Min(calculatedDelayMs + jitterMs, _retrySettings.MaxRetryDelayMs);
                        return TimeSpan.FromMilliseconds(actualDelay);
                    }
                );

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var processedCount = 0;

            foreach (var record in unprocessedRecords)
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await _apiService.ProcessUserRecordAsync(record, processedCount + 1, stopwatch.Elapsed);

                    if (response.IsSuccess)
                    {
                        processedCount++;
                        record.ProcessedIndicator = "Y";
                        await _dataContext.SaveChangesAsync();
                    }
                    else if (response.ShouldRetry)
                    {
                        throw new HttpRequestException("Rate limit exceeded");
                    }
                });
            }

            stopwatch.Stop();
            Console.WriteLine($"Processing completed. {processedCount} records processed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        }
    }
} 