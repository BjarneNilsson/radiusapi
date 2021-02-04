using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net.holmedal;
using Npgsql;

namespace api.Controllers
{
  [ApiController]
  [Route("")]
  public class WeatherForecastController : ControllerBase
  {
   
    [HttpGet("GetUser")]
    [Produces("application/json", new string[] {})]
    public User JsonResult(string usr)
    {
      User user = new User()
      {
        Name = usr,
        Password = (string) null,
        Vlan = new int?()
      };
      NpgsqlConnection connection = new NpgsqlConnection("Host=10.47.1.235;Username=radius;Password=radauth;Database=radius");
      connection.Open();
      NpgsqlCommand npgsqlCommand = new NpgsqlCommand("SELECT value from radcheck where attribute='Cleartext-Password' and username like @sr  ", connection);
      npgsqlCommand.Parameters.AddWithValue("@sr", (object) usr);
      NpgsqlDataReader npgsqlDataReader1 = npgsqlCommand.ExecuteReader();
      while (npgsqlDataReader1.Read())
        user.Password = npgsqlDataReader1.GetString(0);
      npgsqlDataReader1.Close();
      string str1 = "select groupname from radusergroup where username like @dr and priority=0 ";
      npgsqlCommand.CommandText = str1;
      npgsqlCommand.Parameters.AddWithValue("dr", (object) usr);
      NpgsqlDataReader npgsqlDataReader2 = npgsqlCommand.ExecuteReader();
      while (npgsqlDataReader2.Read())
      {
        string str2 = npgsqlDataReader2.GetString(0);
        user.Vlan = new int?(int.Parse(str2.Remove(0, 4)));
      }
      npgsqlDataReader2.Close();
      npgsqlDataReader2.DisposeAsync();
      npgsqlCommand.Dispose();
      connection.Close();
      connection.Dispose();
      return user;
    }

    [HttpPost("AddUser")]
    public void Js([FromBody] User u)
    {
      NpgsqlConnection connection = new NpgsqlConnection("Host=10.47.1.235;Username=radius;Password=radauth;Database=radius");
      connection.Open();
      NpgsqlCommand npgsqlCommand = new NpgsqlCommand("insert into radcheck(username,attribute,op,value) values(@n,'Cleartext-Password',':=', @p );", connection);
      npgsqlCommand.Parameters.AddWithValue("n", (object) u.Name);
      npgsqlCommand.Parameters.AddWithValue("p", (object) u.Password);
      npgsqlCommand.ExecuteNonQuery();
      string str = "insert into  radusergroup (username,groupname,priority) values(@u,@g,0);";
      npgsqlCommand.CommandText = str;
      npgsqlCommand.Parameters.AddWithValue("@u", (object) u.Name);
      npgsqlCommand.Parameters.AddWithValue("g", (object) ("vlan" + u.Vlan.ToString()));
      npgsqlCommand.ExecuteNonQuery();
    }

    [HttpPut("UpdateUser")]
    public void UpdateUser([FromBody] User U)
    {
      NpgsqlConnection connection = new NpgsqlConnection("Host=10.47.1.235;Username=radius;Password=radauth;Database=radius");
      connection.Open();
      NpgsqlCommand npgsqlCommand = new NpgsqlCommand("update radcheck set value=@p where username like @u and attribute like 'Cleartext-Password';", connection);
      npgsqlCommand.Parameters.AddWithValue("u", (object) U.Name);
      npgsqlCommand.Parameters.AddWithValue("p", (object) U.Password);
      npgsqlCommand.ExecuteNonQuery();
      string str = "update radusergroup set groupname=@g where priority=0 and username like @u";
      npgsqlCommand.CommandText = str;
      npgsqlCommand.Parameters.AddWithValue("u", (object) U.Name);
      npgsqlCommand.Parameters.AddWithValue("g", (object) ("vlan" + U.Vlan.ToString()));
      npgsqlCommand.ExecuteNonQuery();
      npgsqlCommand.Dispose();
      connection.Close();
      npgsqlCommand.Dispose();
      connection.Dispose();
    }
    [HttpDelete("DeleteUser")]
    public void DeleteUser( string User)
    {
            NpgsqlConnection connection = new NpgsqlConnection("Host=10.47.1.235;Username=radius;Password=radauth;Database=radius");
            connection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("Delete from  radusergroup where username like @u;",connection);
            npgsqlCommand.Parameters.AddWithValue("u", User);
            npgsqlCommand.ExecuteNonQuery();
            
            string str = "delete from  radcheck where username like @u;";
            npgsqlCommand.CommandText = str;
            npgsqlCommand.Parameters.AddWithValue("u", User);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlCommand.Dispose();
            connection.Close();
            npgsqlCommand.Dispose();
            connection.Dispose();
            //tesw

        }
  }
}
