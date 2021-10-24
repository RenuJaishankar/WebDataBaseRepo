using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace WebDatabase.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AlbumId { get; set; }
        public int MediaTypeId { get; set; }
        public int? GenreId { get; set; }
        public string Composer { get; set; }
        public int Milliseconds { get; set; }
        public int? Bytes { get; set; }
        public int UnitPrice { get; set; }

    }

    public interface ICRUD
    {
        int Create(Track t);                        // C         Create

        Track FindById(int id);                     // R (1)     Read
        List<Track> GetTracks(int skip, int take);  // R (2)

        // List<Track> GetTracks(string query);     // R (3)

        int Update(int id, Track t);                // U         Update

        // int UpdateJust(int id, Track t);
        // int UpdateSafe(int id, Track old, Track Updated);

        int Delete(int id);                         // D         Delete


    }


    public class ChinookCrud : ICRUD
    {

        System.Data.IDbConnection _connection;

        public ChinookCrud()
        {
            _connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=chinook.db");
            _connection.Open();
        }

        public int Create(Track t)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            System.Data.IDbCommand command = _connection.CreateCommand();
            // configured
            command.CommandText = "delete from tracks where trackid = @id";
            // since the sql contains parameters, we need to create and configure the parameters

            // create and configure @id
            System.Data.IDbDataParameter p1 = command.CreateParameter();
            p1.Value = id;
            p1.ParameterName = "@id";
            p1.Direction = ParameterDirection.Input;


            // be sure to add the parameters to the command
            command.Parameters.Add(p1);

            int rv = command.ExecuteNonQuery();
            return rv;
        }

        public Track FindById(int id)
        {

            //Track rv = new Track();  // created as a stub at first. then removed as the implementation was done
            // a command needs to be created, configured and executed

            // created
            System.Data.IDbCommand command = _connection.CreateCommand();
            // configured
            command.CommandText = "select * from tracks where trackid = @id";
            // since the sql contains parameters, we need to create and configure the parameters

            // create and configure @id
            System.Data.IDbDataParameter p1 = command.CreateParameter();
            p1.Value = id;
            p1.ParameterName = "@id";
            p1.Direction = ParameterDirection.Input;


            // be sure to add the parameters to the command
            command.Parameters.Add(p1);
            // the command has been created and configured.  now execute it
            var r = command.ExecuteReader();
            if (r.Read())
            {
                // rv.Composer = (string) r["Composer"]; // this is using the indexer that takes a column name
                // rv.Composer = (string)r[5];  // this is using the indexer that takes a column offset
                // rv.Composer = r.GetString(5);
                TrackMapper tm = new TrackMapper();
                Track t = tm.Map(r);
                r.Close();
                return t; 
            }
            else
            {
                return null;
            }
           // return rv;
        }

        public List<Track> GetTracks(int skip, int take)
        {
            List<Track> rv = new List<Track>();
            // a command needs to be created, configured and executed

            // created
            System.Data.IDbCommand command = _connection.CreateCommand();
            // configured
            command.CommandText = "select * from tracks limit @take offset @skip";
            // since the sql contains parameters, we need to create and configure the parameters

            // create and configure @id
            System.Data.IDbDataParameter p1 = command.CreateParameter();
            p1.Value = skip;
            p1.ParameterName = "@skip";
            p1.Direction = ParameterDirection.Input;
            System.Data.IDbDataParameter p2 = command.CreateParameter();
            p2.Value = take;
            p2.ParameterName = "@take";
            p2.Direction = ParameterDirection.Input;

            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            var r = command.ExecuteReader();
            TrackMapper tm = new TrackMapper();

            while(r.Read())
            {
                Track t = tm.Map(r);
                rv.Add(t);
            }

            r.Close();

            return rv;
        }

        public int Update(int id, Track t)
        {
            System.Data.IDbCommand command = _connection.CreateCommand();
            // configured
            command.CommandText = "update tracks set name = @name, composer=@composer where trackid = @id";
            // since the sql contains parameters, we need to create and configure the parameters

            // create and configure @id
            System.Data.IDbDataParameter p1 = command.CreateParameter();
            p1.Value = t.Name;
            p1.ParameterName = "@name";
            p1.Direction = ParameterDirection.Input;
            System.Data.IDbDataParameter p2 = command.CreateParameter();
            p2.Value = t.Composer;
            p2.ParameterName = "@composer";
            p2.Direction = ParameterDirection.Input;
            System.Data.IDbDataParameter p3 = command.CreateParameter();
            p3.Value = id;
            p3.ParameterName = "@id";
            p3.Direction = ParameterDirection.Input;


            // be sure to add the parameters to the command
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);

            int rv = command.ExecuteNonQuery();
            return rv;
        }
    }


    // this class is a helper to move data from the database record into a c# class
    public class TrackMapper
    {
        int _IdOffset;
        int _NameOffset;  
        // etc.

        // due to lack of time, I will not implement the constuctor to load all the column offsets
        //public TrackMapper(IDataReader r)
        //{
        //    _IdOffset = r.GetOrdinal("TrackID");
        //    if (0 != _IdOffset )
        //    {
        //        throw new Exception($"Message about _idoffset is not as expected. tell what was found and what was expected using squigglies");
        //    }
        //    _NameOffset = r.GetOrdinal("Name)");
        // if (1 != _NameOffset )
        //    {
        //        throw new Exception($"Message about _Nameoffset is not as expected. tell what was found and what was expected using squigglies");
        //    }
        // other column ordinals need to be loaded too
        //}


        public Track Map(IDataReader r)
        {
            Track rv = new Track();

            // the correct way to load is to use the variables
            // rv.id = r.GetInt32(_IdOffset);
            // but here I will simply hard code the integer ordinals
            rv.Id = r.GetInt32(0);
            rv.Name = r.GetString(1);
            if (r.IsDBNull(2))
            {
                rv.AlbumId = null;
            }
            else
            {
                rv.AlbumId = r.GetInt32(2);
            }
            rv.MediaTypeId = r.GetInt32(3);
            if (r.IsDBNull(4))
            {
                rv.GenreId = null;
            }
            else
            {
                rv.GenreId = r.GetInt32(4);
            }
            if (r.IsDBNull(5))
            {
                rv.Composer = null;
            }
            else
            {
                rv.Composer = r.GetString(5);
            }
           
            rv.Milliseconds = r.GetInt32(6);
            if (r.IsDBNull(7))
            {
                rv.Bytes  = null;
            }
            else
            {
                rv.Bytes = r.GetInt32(7);
            }

            rv.UnitPrice = r.GetInt32(8);
            return rv;
        }
    }


}
