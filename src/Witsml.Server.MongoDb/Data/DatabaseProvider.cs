﻿using System;
using System.ComponentModel.Composition;
using System.Configuration;
using MongoDB.Driver;
using PDS.Witsml.Server.Properties;

namespace PDS.Witsml.Server.Data
{
    [Export(typeof(IDatabaseProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DatabaseProvider : IDatabaseProvider
    {
        internal static readonly string DefaultConnectionString = Settings.Default.DefaultConnectionString;
        internal static readonly string DefaultDatabaseName = Settings.Default.DefaultDatabaseName;
        private Lazy<IMongoClient> _client;
        private string _connection;

        [ImportingConstructor]
        public DatabaseProvider(Mapper mapper)
        {
            _client = new Lazy<IMongoClient>(CreateMongoClient);
            mapper.Register();
        }

        public IMongoClient Client
        {
            get { return _client.Value; }
        }

        public IMongoDatabase GetDatabase()
        {
            return Client.GetDatabase(DefaultDatabaseName);
        }

        private IMongoClient CreateMongoClient()
        {
            var settings = ConfigurationManager.ConnectionStrings["MongoDbConnection"];
            var connection = settings == null ? DefaultConnectionString : settings.ConnectionString;
            return new MongoClient(connection);
        }

        private IMongoClient ResetMongoClient()
        {
            return new MongoClient(_connection);
        }

        public void ResetConnection(string connection)
        {
            _connection = connection;
            _client = new Lazy<IMongoClient>(ResetMongoClient);
        }
    }
}
