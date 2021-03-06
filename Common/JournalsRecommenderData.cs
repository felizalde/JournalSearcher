using Microsoft.Extensions.Configuration;
using Common.Models;
using System.Collections.Generic;
using Npgsql;
using System;
using Z.Dapper.Plus;
using Dapper;
using Common.Models.Dataset;
using System.Linq;

namespace Common;


public class JournalsRecommenderData
{
    private readonly IConfiguration _config;

    public JournalsRecommenderData(IConfiguration configuration)
    {
        _config = configuration;
        DapperPlusManager.Entity<Journal>().Table("journal");
        DapperPlusManager.Entity<JournalMetric>().Table("journal_metrics");
        DapperPlusManager.Entity<VenueInfo>().Table("venue_info");
        DapperPlusManager.Entity<PaperInfo>().Table("paper_info");
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


    public IEnumerable<Journal> GetAllJournals(int version = 1, string editorial = null)
    {
        try
        {
            var mapped = new Dictionary<Guid, Journal>();
            var query = string.IsNullOrEmpty(editorial) ? @"SELECT * FROM journal 
                        LEFT JOIN journal_metrics on journal.id = journal_metrics.journalid
                        WHERE version = @version" : @"SELECT * FROM journal 
                        LEFT JOIN journal_metrics on journal.id = journal_metrics.journalid
                        WHERE version = @version and editorial = @editorial";
            var conn = Connection;
            var journals = conn.Query<Journal, JournalMetric, Journal>(query, (journal, metric) =>
                                                                       {
                                                                           if (!mapped.TryGetValue(journal.Id, out Journal toMap))
                                                                           {
                                                                               toMap = journal;
                                                                               toMap.Metrics = new();
                                                                               mapped.Add(journal.Id, toMap);
                                                                           }
                                                                           if (metric is { Value: > 0})
                                                                           {
                                                                                toMap.Metrics.Add(metric);
                                                                           }
                                                                           return toMap;
                                                                       }, new { version, editorial }).Distinct();
                                                      
            return journals;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public void InsertBulkVenues(IEnumerable<VenueInfo> venues)
    {
        try
        {
            var conn = Connection;
            conn.BulkInsert(venues);

        } catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }


    public void InsertBulkPaper(IEnumerable<PaperInfo> papers)
    {
        try
        {
            var conn = Connection;
            conn.BulkInsert(papers);

        } catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }


    public IEnumerable<VenueInfo> GetAllVenueInfoExistingInJournal()
    {
        try
        {
            var conn = Connection;
            var venues = conn.Query<VenueInfo>("SELECT DISTINCT venue_info.* FROM venue_info inner join journal on venue_info.displayname = journal.title");

            return venues;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }


    public IEnumerable<T> ExecQuery<T>(string query, object param = null)
    {
        try
        {
            var conn = Connection;
            var result = conn.Query<T>(query, param);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

}