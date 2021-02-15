Api to manage database for freeradius (https termineted on reverse proxy in front of app)
api assume use of postgresql
<br>
'''xml
Schema for Parameters.json
"Logging": {
    "LogLevel": {
      "Default":  "Warning"
    }
  },
  "Db": {
    "ConnetionString": "Host=dbhostname;Username=dbusername;Password=dbpassword;Database=db"
  }
}
'''
-- still missing Autentication, at precent anyone can create. retreve,,update or delete any user--
-lisence (not that anyone would want this in it current state) BSD 3 clause lisence as long as is does not conflict wit the licence of any dependencies

Dependenceies 
Microsoft.AspNetCore.Mvc
net.holmedal (https://github.com/BjarneNilsson/RadiusClientLib)
Npgsql (nuget)
System.IO;
Microsoft.Extensions.Configuration (nuget)
''''


