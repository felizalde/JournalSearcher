using Microsoft.Extensions.Configuration;
using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using System;
using Z.Dapper.Plus;
using Dapper;

namespace Common;


public class JournalsRecommenderData
{
    private readonly IConfiguration _config;

    public JournalsRecommenderData(IConfiguration configuration)
    {
        _config = configuration;
        DapperPlusManager.Entity<Journal>().Table("journal");
        DapperPlusManager.Entity<JournalMetric>().Table("journal_metrics");
    }


    private NpgsqlConnection _conn;

    public NpgsqlConnection Connection
    {
        get
        {
            if (_conn == null)
            {
                _conn = new NpgsqlConnection(_config.GetConnectionString("MainDB"));
                try
                {
                    _conn.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error connecting to database", ex);
                }
            }
            return _conn;
        }
    }


    public void InsertBulkJournals(IEnumerable<Journal> journals)
    {
        try
        {

            var conn = Connection;
            conn.BulkInsert(journals)
                        .ThenForEach(x => x.Metrics.ForEach(y => y.JournalId = x.Id))
                        .ThenBulkInsert(x => x.Metrics);
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }


    public IEnumerable<Journal> GetAllJournals(int version = 1)
    {
        try
        {

            var conn = Connection;
            var journals = conn.Query<Journal>("SELECT * FROM journal WHERE version = @version", new { version });

            return journals;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

}