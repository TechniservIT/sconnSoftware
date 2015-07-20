using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iotDbConnector.DAL;
using System.Linq;
using System.Diagnostics;
using iotNoSqlDatabase;
using System.Collections.Generic;

namespace iotDatabaseTester
{
	[TestClass]
	public class NoSqlDboUnitTest
	{
		[TestMethod]
		public void TestMethod1()
		{
		}


		[TestMethod]
		public void TestRavenDbCRUD()
		{
			RavenNoSqlDataSource<Device> db = new RavenNoSqlDataSource<Device>();
			db.Add(new Device());
			var res = db.GetDevice();
			Assert.IsTrue(res != null);
		}

		[TestMethod]
		public void TestRavenDbReadLoad()
		{
			int ReadTestInterations = 250;
			int maxQueryTimeMs = 25;
			RavenNoSqlDataSource<Device> db = new RavenNoSqlDataSource<Device>();
			Stopwatch watch = new Stopwatch();
			watch.Start();
			for (int i = 0; i<ReadTestInterations; i++)
			{
				var res = db.GetDevice();
			}
			watch.Stop();
			double TimePerQueryMs = (double)watch.ElapsedMilliseconds / (double)ReadTestInterations;
			Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
		}

		[TestMethod]
		public void TestRedisDbWriteLoad()
		{
			int ReadTestInterations = 250;
			int maxQueryTimeMs = 25;
			RedisNoSqlDataSource<Device> db = new RedisNoSqlDataSource<Device>();
			Stopwatch watch = new Stopwatch();
			watch.Start();
			for (int i = 0; i < ReadTestInterations; i++)
			{
				var res = db.Add(new Device());
			}
			watch.Stop();
			double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
			Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
		}

		[TestMethod]
		public void TestRedisDbRWLoad()
		{
			int ReadTestInterations = 250;
			int maxQueryTimeMs = 25;
			RedisNoSqlDataSource<Device> db = new RedisNoSqlDataSource<Device>();
			Stopwatch watch = new Stopwatch();
			watch.Start();
			string id = db.Add(new Device { DeviceName = Guid.NewGuid().ToString() });
			for (int i = 0; i < ReadTestInterations; i++)
			{
				var stored = db.GetById(id);
				if (stored == null)
				{
					Assert.Fail();
				}
			}
			watch.Stop();
			double TimePerQueryMs = (double)watch.ElapsedMilliseconds / (double)ReadTestInterations;
			Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
		} 


		[TestMethod]
		public void TestRedisWriteLoadFullEntity()
		{
			int TestDeviceNo = 10;
			int TestDeviceActionNo = 10;
			int TestDevicePropertyNo = 10;

			//add domain
			iotDomain domain = new iotDomain();
			domain.DomainName = "devDomain";
		   

			//add device type 
			DeviceType type = new DeviceType();
			type.TypeName = "sconnMB";
			type.TypeDescription = "desc";
			type.VisualRepresentationURL = "URL";

			//add location
			Location loc = new Location();
			loc.Lat = 11.11;
			loc.Lng = 22.22;
			loc.LocationName = "Wisla";
			domain.Locations =new List<Location>();
			domain.Locations.Add(loc);


			//add site
			Site devsite = new Site();
			devsite.Domain = domain;
			devsite.siteLocation = loc;
			devsite.SiteName = "devSite";
			devsite.Devices = new List<Device>();
			domain.Sites = new List<Site>();
			domain.Sites.Add(devsite);


			//repeat X times
			//add device with properties & actions
			for (int i = 0; i < TestDeviceNo; i++)
			{
				Device dev = new Device();
				dev.Properties = new List<DeviceProperty>();
				dev.Actions = new List<DeviceAction>();

				//Device credentials
				DeviceCredentials cred = new DeviceCredentials();
				cred.PasswordExpireDate = DateTime.MaxValue;
				cred.PermissionExpireDate = DateTime.MaxValue;
				cred.Username = "admin";
				cred.Password = "testpass";

				EndpointInfo info = new EndpointInfo();
				info.Port = 9898;
				info.Hostname = "192.168.1.1";
				info.SupportsSconnProtocol = true;
				info.Devices.Add(dev);
				

				//Device endpoint
				dev.EndpInfo = info;
				dev.Credentials = cred;
				dev.DeviceName = "dev" + i.ToString();
				dev.Type = type;
				dev.Site = devsite;

				//add sample Actions
				ParameterType paramtype = new ParameterType();
				paramtype.Name = "ActionParam";
				for (int a = 0; a < TestDeviceActionNo; a++)
				{
					DeviceAction act = new DeviceAction();
					act.ActionName = "Act" + a.ToString();
					act.Device = dev;

					//required params
					ActionParameter param = new ActionParameter();
					param.Type = paramtype;
					param.Value = "";
					param.Action = act;
					act.RequiredParameters = new List<ActionParameter>();
					act.RequiredParameters.Add(param);

					//result params
                    DeviceActionResult param2 = new DeviceActionResult();
					param2.Type = paramtype;
					param2.Value = "";
					param2.Action = act;
                    act.ResultParameters = new List<DeviceActionResult>();
					act.ResultParameters.Add(param2);

					dev.Actions.Add(act);
				}

				//add Sample Proprties
				for (int p = 0; p < TestDevicePropertyNo; p++)
				{
					DeviceProperty prop = new DeviceProperty();
					prop.PropertyName = "Prop" + p.ToString();
					prop.Device = dev;
					
					//result params
					DeviceParameter param2 = new DeviceParameter();
					param2.Type = paramtype;
					param2.Value = "";
					param2.Property = prop;
					prop.ResultParameters = new List<DeviceParameter>();
					prop.ResultParameters.Add(param2);

					dev.Properties.Add(prop);
				}

				//add device to site
				devsite.Devices.Add(dev);
				
			}

			RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
			string id = db.Add(domain); //store domain
			
			//domain read test
			int ReadTestInterations = 250;
			int maxQueryTimeMs = 50;
		   
			Stopwatch watch = new Stopwatch();
			watch.Start();
			for (int i = 0; i < ReadTestInterations; i++)
			{
				var stored = db.GetById(id);
				if (stored == null)
				{
					Assert.Fail();
				}
			}
			watch.Stop();
			double TimePerQueryMs = (double)watch.ElapsedMilliseconds / (double)ReadTestInterations;
			Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);

		}


	}
}
