using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServerAPI.Classes;
using TurkmenBuildProjectServer.Classes;

namespace ServerAPI.Controllers
{
    public class DatabaseController : Controller
    {
        private BuildCompanyDBContainer Database = new BuildCompanyDBContainer();

        [HttpPost]
        [Route("database/signup")]
        public string SignUp(User user, UserProfile userProfile)
        {
            Image UserPicture = null;
            string SavePath;
            if (userProfile.Picture == "default")
            {
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\users\\Мужик-строитель.jpg";
            }
            else
            {
                UserPicture = ByteConverter.ConvertToImage(Convert.FromBase64String(userProfile.Picture));
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\users\\userpic_{Guid.NewGuid()}.jpeg";
                UserPicture.Save(SavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            

            var userFromDB = (from u in Database.UserSet
                                where u.Login == user.Login
                                select u);

            if (!userFromDB.Any())
            {
                userProfile.Picture = SavePath;
                user.UserProfile = userProfile;
                userProfile.User = user;

                Database.UserSet.Add(user);
                Database.UserProfileSet.Add(userProfile);
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    {"Message", $"Успешная регистрация пользователя с логином {user.Login}" },
                    {"Success", 1 },
                    {"Results", new Dictionary<string, object>()
                    {
                        {"Id", user.Id}
                    } }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    {"Message", "Регистрация пользователя не удалась" },
                    {"Success", 1 },
                    {"Results", "" }
                });
            }          
        }

        [HttpGet]
        [Route("database/getusers")]
        public string GetUsers()
        {
            var UserList = Database.UserSet.ToList();
            List<string> Users = new List<string>();

            foreach (User user in UserList)
            {
                Users.Add(user.ToJson());
            }

            var JsonUsers = JsonConvert.SerializeObject(Users);

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", "Передача пользователей" },
                { "Success", 1 },
                { "Results", JsonUsers }
            });
        }

        [HttpGet]
        [Route("database/getuser")]
        public string GetUser(int Id)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", User.FirstOrDefault().ToJson()},
                    { "Message", "Пользователь найден" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Пользователь не найден" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPost]
        [Route("database/login")]
        public string LogIn(string Login, string Password)
        {
            var User = (from User u in Database.UserSet
                        where u.Login == Login && u.Password == Password
                        select u);

            if (User.Any())
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", User.FirstOrDefault().ToJson()},
                    { "Message", "Пользователь найден" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Логин или пароль не верные" },
                    { "Success", 0 }
                });
            }
        }

        [HttpGet]
        [Route("database/getuser")]
        public string GetUser(string Login)
        {
            var User = (from User u in Database.UserSet
                        where u.Login == Login
                        select u);

            if (User.Any())
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", User.FirstOrDefault().ToJson()},
                    { "Message", "Пользователь найден" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Пользователь не найден" },
                    { "Success", 0 }
                });
            }
        }

        [HttpDelete]
        [Route("database/deleteuser/{Id}")]
        public string DeleteUser(int Id)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                var UserLogin = User.FirstOrDefault().Login;
                Database.UserProfileSet.Remove(User.FirstOrDefault().UserProfile);
                Database.UserSet.Remove(User.FirstOrDefault());
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", UserLogin},
                    { "Message", "Пользователь удален" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Пользователь не найден" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changeuserlogin")]
        public string ChangeUserLogin(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Login == Value
                        select u);

            if (!User.Any())
            {
                var PutUser = Database.UserSet.Find(Id);
                PutUser.Login = Value;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.Login},
                    { "Message", "Пользователь изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Логин занят другим пользователем" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changeuseremail")]
        public string ChangeUserEmail(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                var PutUser = Database.UserSet.Find(Id);
                PutUser.UserProfile.Email = Value;
                Database.Entry(PutUser).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.UserProfile.Email},
                    { "Message", "Пользователь изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changeuserpicture")]
        public string ChangeUserPicture(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                Image UserPicture = ByteConverter.ConvertToImage(Convert.FromBase64String(Value));
                string SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\users\\userpic_{Guid.NewGuid()}.jpeg";
                UserPicture.Save(SavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                var PutUser = Database.UserSet.Find(Id);
                PutUser.UserProfile.Picture = SavePath;
                Database.Entry(PutUser).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.Login},
                    { "Message", $"Картинка пользователя {PutUser.Login} изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changeusername")]
        public string ChangeUsername(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                var PutUser = Database.UserSet.Find(Id);
                PutUser.UserProfile.Username = Value;
                Database.Entry(PutUser).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.UserProfile.Username},
                    { "Message", "Пользователь изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changeuserrole")]
        public string ChangeUserRole(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                var PutUser = Database.UserSet.Find(Id);
                PutUser.Role = Value;
                Database.Entry(PutUser).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.Role},
                    { "Message", "Пользователь изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changeuserphone")]
        public string ChangeUserPhone(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                var PutUser = Database.UserSet.Find(Id);
                PutUser.UserProfile.Phone = Value;
                Database.Entry(PutUser).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.UserProfile.Phone},
                    { "Message", "Пользователь изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/changerentdurationtime")]
        public string ChangeRentDurationTime(int Id, string Value)
        {
            var Rent = (from Rent r in Database.RentSet
                        where r.Id == Id
                        select r);

            if (Rent.Any())
            {
                var PutRent = Database.RentSet.Find(Id);
                PutRent.DurationTime = Convert.ToInt32(Value);
                Database.Entry(PutRent).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutRent.RentType},
                    { "Message", "Аренда изменена" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPost]
        [Route("database/createvehiclerentoffer")]
        public string CreateVehicleRentOffer(Vehicles vehicles)
        {

            Image VehicleImage = null;
            string SavePath;
            if (vehicles.Picture == "default")
            {
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\vehicles\\default-gazel.jpg";
            }
            else
            {
                VehicleImage = ByteConverter.ConvertToImage(Convert.FromBase64String(vehicles.Picture));
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\vehicles\\vehicle_{Guid.NewGuid()}.jpeg";
                VehicleImage.Save(SavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            vehicles.MaxWeight = Convert.ToDouble(vehicles.MaxWeight);
            vehicles.GasolineLevel = Convert.ToDouble(vehicles.GasolineLevel);
            vehicles.RentPrice = Convert.ToDouble(vehicles.RentPrice);

            Rent rent = new Rent("Аренда техники");
            rent.Vehicles = vehicles;
            vehicles.Rent = rent;
            vehicles.Picture = SavePath;

            Database.RentSet.Add(rent);
            Database.VehiclesSet.Add(vehicles);
            Database.SaveChanges();

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                {"Message", $"Успешное добавление техники с наименованием {vehicles.VehicleName}" },
                {"Success", 1 },
                {"Results", new Dictionary<string, object>()
                {
                    {"Id", vehicles.Id}
                } }
            });
            
        }

        [HttpPost]
        [Route("database/creategoodrentoffer")]
        public string CreateGoodRentOffer(Goods goods, string RentType)
        {
            Image GoodImage = null;
            string SavePath;
            if (goods.Picture == "default")
            {
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\goods\\default-good.jpg";
            }
            else
            {
                GoodImage = ByteConverter.ConvertToImage(Convert.FromBase64String(goods.Picture));
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\goods\\good_{Guid.NewGuid()}.jpeg";
                GoodImage.Save(SavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            Rent rent = new Rent(RentType);
            rent.Goods = goods;
            goods.Rent = rent;
            goods.Picture = SavePath;

            Database.RentSet.Add(rent);
            Database.GoodsSet.Add(goods);
            Database.SaveChanges();

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                {"Message", $"Успешное добавление товара с наименованием {goods.GoodName}" },
                {"Success", 1 },
                {"Results", new Dictionary<string, object>()
                {
                    {"Id", goods.Id}
                } }
            });          
        }

        [HttpPost]
        [Route("database/createproject")]
        public string CreateProject(Project project)
        {
            Image ProjectImage = null;           
            string SavePath;
            if (project.Picture == "default")
            {
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\projects\\chertezh-doma.jpg";
            }
            else
            {
                ProjectImage = ByteConverter.ConvertToImage(Convert.FromBase64String(project.Picture));
                SavePath = Directory.GetCurrentDirectory() + $"\\pictures\\projects\\project_{Guid.NewGuid()}.jpeg";
                ProjectImage.Save(SavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            project.Picture = SavePath;

            Database.ProjectSet.Add(project);
            Database.SaveChanges();

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                {"Message", $"Успешное добавление проекта с наименованием {project.ProjectType}" },
                {"Success", 1 },
                {"Results", new Dictionary<string, object>()
                {
                    {"Id", project.Id}
                } }
            });           
        }

        [HttpGet]
        [Route("database/getvehicles")]
        public string GetVehicles()
        {
            var VehiclesList = Database.VehiclesSet.ToList();
            List<string> Vehicles = new List<string>();

            foreach (Vehicles vehicle in VehiclesList)
            {
                Vehicles.Add(vehicle.ToJson());
            }

            var JsonVehicles = JsonConvert.SerializeObject(Vehicles);

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", "Передача техники" },
                { "Success", 1 },
                { "Results", JsonVehicles }
            });
        }

        [HttpGet]
        [Route("database/getrents")]
        public string GetRents()
        {
            var RentList = Database.RentSet.ToList();
            List<string> Rents = new List<string>();

            foreach (Rent rent in RentList)
            {
                Rents.Add(rent.ToJson());
            }

            var JsonRents = JsonConvert.SerializeObject(Rents);

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", "Передача предложений аренды" },
                { "Success", 1 },
                { "Results", JsonRents }
            });
        }
        [HttpGet]
        [Route("database/getprojects")]
        public string GetProjects()
        {
            var ProjectList = Database.ProjectSet.ToList();
            List<string> Projects = new List<string>();

            foreach (Project project in ProjectList)
            {
                Projects.Add(project.ToJson());
            }

            var JsonProjects = JsonConvert.SerializeObject(Projects);

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", "Передача предложений аренды" },
                { "Success", 1 },
                { "Results", JsonProjects }
            });
        }

        [HttpDelete]
        [Route("database/deleteproject/{Id}")]
        public string DeleteProject(int Id)
        {
            var Project = (from Project p in Database.ProjectSet
                        where p.Id == Id
                        select p);

            if (Project.Any())
            {
                var ProjectName = Project.FirstOrDefault().ProjectType;
                Database.ProjectSet.Remove(Project.FirstOrDefault());
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", ProjectName},
                    { "Message", "Проект для строительства удален" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Проект не найден" },
                    { "Success", 0 }
                });
            }
        }

        [HttpDelete]
        [Route("database/deleterent/{Id}")]
        public string DeleteRent(int Id)
        {
            var Rent = (from Rent r in Database.RentSet
                           where r.Id == Id
                           select r);

            if (Rent.Any())
            {
                var RentName = Rent.FirstOrDefault().RentType;
                Database.RentSet.Remove(Rent.FirstOrDefault());
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", RentName},
                    { "Message", "Предложение для аренды удалено" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Предложение аренды не найдено" },
                    { "Success", 0 }
                });
            }
        }

        [HttpPut]
        [Route("database/resetpassword")]
        public string ResetPassword(int Id, string Value)
        {
            var User = (from User u in Database.UserSet
                        where u.Id == Id
                        select u);

            if (User.Any())
            {
                var PutUser = Database.UserSet.Find(Id);
                PutUser.Password = Value;
                Database.Entry(PutUser).State = EntityState.Modified;
                Database.SaveChanges();

                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", PutUser.Login},
                    { "Message", $"Пароль пользователя {PutUser.Login} изменен" },
                    { "Success", 1 }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    { "Results", "" },
                    { "Message", "Не удалось изменить" },
                    { "Success", 0 }
                });
            }
        }

        [HttpGet]
        [Route("database/getorders/{Id}")]
        public string GetOrders(int Id)
        {
            var User = Database.UserSet.Find(Id);
            var UserOrders = User.Order.ToList();
            List<string> Orders = new List<string>();

            foreach (Order order in UserOrders)
            {
                Orders.Add(order.ToJson());
            }

            var JsonProjects = JsonConvert.SerializeObject(Orders);

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", $"Передача истории заказов пользователя {User.Login}" },
                { "Success", 1 },
                { "Results", JsonProjects }
            });
        }

        [HttpPost]
        [Route("database/makerentorder")]
        public string MakeRentOrder(int Id, int Rent_Id)
        {
            var Rent = Database.RentSet.Find(Rent_Id);
            var User = Database.UserSet.Find(Id);

            Order order = new Order()
            {
                OrderType = Rent.RentType,
                DateOfOrder = DateTime.Now,
                Rent = Rent,
                User = User,
                UserId = Id
            };

            Database.OrderSet.Add(order);
            Database.SaveChanges();

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", $"Успешно оформлен заказ для пользователя {User.Login}" },
                { "Success", 1 },
                { "Results", "" }
            });
        }
        [HttpPost]
        [Route("database/makeprojectorder")]
        public string MakeProjectOrder(int Id, int Project_Id)
        {
            var Project = Database.ProjectSet.Find(Project_Id);
            var User = Database.UserSet.Find(Id);

            Order order = new Order()
            {
                OrderType = "Проект на строительство",
                DateOfOrder = DateTime.Now,
                Project = Project,
                User = User,
                UserId = Id
            };

            Database.OrderSet.Add(order);
            Database.SaveChanges();

            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "Message", $"Успешно оформлен заказ для пользователя {User.Login}" },
                { "Success", 1 },
                { "Results", "" }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Database.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
