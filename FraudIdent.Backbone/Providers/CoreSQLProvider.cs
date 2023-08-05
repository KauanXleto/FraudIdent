using Dapper;
using FraudIdent.Backbone.BusinessEntities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace FraudIdent.Backbone.Providers
{
    public class CoreSQLProvider
    {
        private IConfiguration _config;
        private static string connString;
        private SqlConnection conn;

        public CoreSQLProvider(IConfiguration _configuuration)
        {
            _config = _configuuration;
            connString = _config.GetConnectionString("FraudIdentDB");
        }

        #region BalanceInfo

        public BalanceInfo GetBalanceInfo()
        {
            var result = new BalanceInfo();

            using(var connection = new SqlConnection(connString))
            {
                result = connection.QueryFirstOrDefault<BalanceInfo>("Select * from BalanceInfo");
            }

            return result;
        }

        public void UpdateBalanceInfo(BalanceInfo entity)
        {

            using (var connection = new SqlConnection(connString))
            {
                connection.Execute($@"Update BalanceInfo 
                                set
                                    Length = {entity.Length.ToString().Replace(",", ".")},
                                    Width = {entity.Width.ToString().Replace(",", ".")},
                                    DistanceScaleCam1 = {entity.DistanceScaleCam1.ToString().Replace(",", ".")},
                                    DistanceScaleCam2 = {entity.DistanceScaleCam2.ToString().Replace(",", ".")}

                                    where Id = {entity.Id.ToString()}");
            }
        }

        public void InsertBalanceInfo(BalanceInfo entity)
        {

            using (var connection = new SqlConnection(connString))
            {
                entity.Id = connection.QuerySingle<int>($@"Insert into BalanceInfo (Length, Width, DistanceScaleCam1, DistanceScaleCam2)  OUTPUT INSERTED.Id
                                    values({entity.Length.ToString().Replace(",", ".")},
                                           {entity.Width.ToString().Replace(",", ".")},
                                           {entity.DistanceScaleCam1.ToString().Replace(",", ".")},
                                           {entity.DistanceScaleCam2.ToString().Replace(",", ".")})");
            }
        }

        #endregion

        #region Truck

        public List<Truck> GetTrucks()
        {
            var result = new List<Truck>();

            using (var connection = new SqlConnection(connString))
            {
                result = connection.Query<Truck>("Select * from Truck").ToList();
            }

            return result;
        }
        public List<Truck> GetTrucks(Truck entity)
        {

            var sql = $@"
                Select * from Truck
                where 1 = 1
                {(entity.Id > 0 ? " and Id = " + entity.Id.ToString() : "")}
                {(!string.IsNullOrWhiteSpace(entity.Name) ? " and Name like '%" + entity.Name + "%'" : "")}
            ";

            var result = new List<Truck>();


            using (var connection = new SqlConnection(connString))
            {
                result = connection.Query<Truck>(sql).ToList();
            }

            return result;
        }

        public int UpdateTruck(Truck entity)
        {

            var sql = $@"Update Truck 
                                set
                                    Name = '{entity.Name}'

                                    where Id = {entity.Id.ToString()}";

            var result = 0;
            using (var connection = new SqlConnection(connString))
            {
                result = connection.Execute(sql);
            }

            return result;
        }

        public int InsertTruck(Truck entity)
        {

            var sql = $@"insert into Truck(Name) OUTPUT INSERTED.Id
                                values('{entity.Name}')";


            using (var connection = new SqlConnection(connString))
            {
                entity.Id = connection.QuerySingle<int>(sql);
            }
            return entity.Id;
        }
        public TruckParam GetTruckParam(int TruckId)
        {

            var sql = $@"
                Select * from TruckParam
                where Id = {TruckId.ToString()}            
            ";

            var result = new TruckParam();
            using (var connection = new SqlConnection(connString))
            {
                result = connection.QueryFirstOrDefault<TruckParam>(sql);
            }

            return result;
        }

        public int UpdateTruckParam(TruckParam entity)
        {

            var sql = $@"Update TruckParam 
                                set
                                    TruckId = {entity.TruckId.ToString()},
                                    Height = {entity.Height.ToString().Replace(",", ".")},
                                    Length = {entity.Length.ToString().Replace(",", ".")},
                                    Width = {entity.Width.ToString().Replace(",", ".")}

                                    where Id = {entity.Id.ToString()}";


            var result = 0;
            using (var connection = new SqlConnection(connString))
            {
                result = connection.Execute(sql);
            }

            return result; 
        }

        public int InsertTruckParam(TruckParam entity)
        {

            var sql = $@"insert into TruckParam(TruckId, Height, Length, Width)  OUTPUT INSERTED.Id
                                values({entity.TruckId.ToString()},
                                       {entity.Height.ToString().Replace(",", ".")},
                                       {entity.Length.ToString().Replace(",", ".")},
                                       {entity.Width.ToString().Replace(",", ".")})";

            using (var connection = new SqlConnection(connString))
            {
                entity.Id = connection.QuerySingle<int>(sql);
            }

            return entity.Id;
        }

        #endregion


        #region LobInfo

        public List<LobInfo> GetLobInfo()
        {

            var sql = $@"
                Select top 10 * from LobInfo order by CreateDate Desc";

            var result = new List<LobInfo>();
            using (var connection = new SqlConnection(connString))
            {
                result = connection.Query<LobInfo>(sql).ToList();
            }

            return result;
        }

        public List<LobInfo> GetLobInfo(LobInfo entity)
        {

            var sql = $@"
                Select * from LobInfo
                where 1 = 1
                {(entity.Id > 0 ? " and Id = " + entity.Id.ToString() : "")}
                {(entity.TruckId > 0 ? " and TruckId = " + entity.TruckId.ToString() : "")}
                {(entity.HasError != null ? " and HasError = " + (entity.HasError == true ? "1" : "0") : "")}
                {(entity.HasSuccess != null ? " and HasSuccess = " + (entity.HasSuccess == true ? "1" : "0") : "")}
            ";

            var result = new List<LobInfo>();

            using (var connection = new SqlConnection(connString))
            {
                result = connection.Query<LobInfo>(sql).ToList();
            }

            return result;
        }

        public int UpdateLobInfo(LobInfo entity)
        {

            var sql = $@"Update LobInfo 
                                set
                                    TruckId = {entity.TruckId.ToString()},
                                    HasError = {(entity.HasError != null ? (entity.HasError == true ? "1" : "0") : "")},
                                    HasSuccess = {(entity.HasSuccess != null ? (entity.HasSuccess == true ? "1" : "0") : "")},
                                    MessageError = '{entity.MessageError}',
                                    BackImageTruck = '{entity.BackImageTruck}',
                                    SideImageTruck = '{entity.SideImageTruck}'

                                    where Id = {entity.Id.ToString()}";
            var result = 0;
            using (var connection = new SqlConnection(connString))
            {
                result = connection.Execute(sql);
            }

            return result;
        }
        public int InsertLobInfo(LobInfo entity)
        {

            var sql = $@"insert into LobInfo(TruckId, CreateDate, HasError, HasSuccess, MessageError, BackImageTruck, SideImageTruck)  OUTPUT INSERTED.Id
                                values({entity.TruckId.ToString()},
                                       '{DateTime.Now.ToString()}',
                                       {(entity.HasError != null ? (entity.HasError == true ? "1" : "0") : "")},
                                       {(entity.HasSuccess != null ? (entity.HasSuccess == true ? "1" : "0") : "")},
                                       '{entity.MessageError}',
                                       '{entity.BackImageTruck}',
                                       '{entity.SideImageTruck}')";


            using (var connection = new SqlConnection(connString))
            {
                entity.Id = conn.QuerySingle(sql);
            }

            return entity.Id;
        }

        #endregion
    }
}
