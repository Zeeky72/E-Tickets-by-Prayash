using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Cinema
                if (!context.Cinemas.Any())
                {
                    context.Cinemas.AddRange(new List<Cinema>()
                    {
                        new Cinema()
                        {
                            Name = "Penn Cinema",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-1.jpeg",
                            Description = "Established in 2006. Penn Cinema is a family-owned and independently-operated movie theater. "
                        },
                        new Cinema()
                        {
                            Name = "Red Chillies Entertainment",
                            Logo = "https://upload.wikimedia.org/wikipedia/en/2/21/Red_Chillies_Entertainment_logo.png",
                            Description = "Red Chillies Entertainment is an Indian visual effects, production and distribution company established by actor Shah Rukh Khan and his wife Gauri Khan in 2002."
                        },
                        new Cinema()
                        {
                            Name = "Paramount Cinemas",
                            Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Paramount_Global_Logo.svg/330px-Paramount_Global_Logo.svg.png",
                            Description = "Paramount Media Networks (founded as MTV Networks in 1984 and known under this name until 2011) is an American mass media division."
                        },
                        new Cinema()
                        {
                            Name = "Dharma Production",
                            Logo = "https://upload.wikimedia.org/wikipedia/en/thumb/0/02/Dharma_Production_logo.png/300px-Dharma_Production_logo.png",
                            Description = "Dharma Productions Pvt. Ltd. is an Indian film production and distribution company established by Yash Johar in 1979."
                        },
                        new Cinema()
                        {
                            Name = "Eros International",
                            Logo = "https://upload.wikimedia.org/wikipedia/en/e/ec/Eros_International_logo.jpg",
                            Description = "Eros International Media Ltd (also known as Eros India) is an Indian motion picture production and distribution company, based and originated in Mumbai, India."
                        },
                    });
                    context.SaveChanges();
                }
                //Actors
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(new List<Actor>()
                    {
                        new Actor()
                        {
                            FullName = "Rajinikanth",
                            Bio = "This is the Bio of the first actor",
                            ProfilePictureURL = "Shivaji Rao Gaikwad(born 12 December 1950), known professionally as Rajinikanth."

                        },
                        new Actor()
                        {
                            FullName = "Akshay Kumar",
                            Bio = "Rajiv Hari Om Bhatia (born 9 September 1967) known professionally as Akshay Kumar.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2e/Akshay_Kumar.jpg/330px-Akshay_Kumar.jpg"
                        },
                        new Actor()
                        {
                            FullName = "Samantha Ruth Prabhu",
                            Bio = "Samantha began her professional film career with Gautham Vasudev Menon's Telugu film.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/86/Samantha_from_SVSC_movie.png/330px-Samantha_from_SVSC_movie.png"
                        },
                        new Actor()
                        {
                            FullName = "Kareena Kapoor",
                            Bio = "Kareena Kapoor Khan (born 21 September 1980) is an Indian actress .",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/66/Kareena_Kapoor_at_TOIFA16.jpg/330px-Kareena_Kapoor_at_TOIFA16.jpg"
                        },
                        new Actor()
                        {
                            FullName = "Yash",
                            Bio = "Naveen Kumar Gowda (born 8 January 1986), better known by his stage name Yash.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/Yash_promoting_KGF_2.jpg/330px-Yash_promoting_KGF_2.jpg"
                        },
                        new Actor()
                        {
                            FullName = "Shah Rukh Khan",
                            Bio = "Shah Rukh Khan (born 2 November 1965), also known by the initialism SRK.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/Shah_Rukh_Khan_graces_the_launch_of_the_new_Santro.jpg/330px-Shah_Rukh_Khan_graces_the_launch_of_the_new_Santro.jpg"
                        }
                    });
                    context.SaveChanges();
                }
                //Producers
                if (!context.Producers.Any())
                {
                    context.Producers.AddRange(new List<Producer>()
                    {
                        new Producer()
                        {
                            FullName = "Tom Cruise",
                            Bio = "Tom Cruise, is an American actor and producer.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/33/Tom_Cruise_by_Gage_Skidmore_2.jpg/330px-Tom_Cruise_by_Gage_Skidmore_2.jpg"

                        },
                        new Producer()
                        {
                            FullName = "Jerome Leon Bruckheimer ",
                            Bio = "Jerome Leon Bruckheimer (born September 21, 1943) is an American film and television producer.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/JerryBruckheimerHWOFJune2013.jpg/330px-JerryBruckheimerHWOFJune2013.jpg"
                        },
                        new Producer()
                        {
                            FullName = "Karan Johar",
                            Bio = "Karan Johar  is an Indian filmmaker and television personality.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bb/Karan_Johar_walks_the_ramp_for_Shane_and_Falguni_Peacock.jpg/330px-Karan_Johar_walks_the_ramp_for_Shane_and_Falguni_Peacock.jpg"
                        },
                        new Producer()
                        {
                            FullName = "Siddharth Roy Kapur",
                            Bio = "Siddharth Roy Kapur (born 2 August 1974) is an Indian film producer and the founder and managing director of Roy Kapur Films.",
                            ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/en/thumb/8/8f/Siddharth_Roy_Kapur_1.jpg/330px-Siddharth_Roy_Kapur_1.jpg"
                        },
                        new Producer()
                        {
                            FullName = "Basil Iwanyk",
                            Bio = "Basil William Iwanyk (born January 4, 1970) is an American film producer.",
                            ProfilePictureURL = "http://dotnethow.net/images/producers/producer-1.jpeg"
                        }
                    });
                    context.SaveChanges();
                }
                //Movies
                if (!context.Movies.Any())
                {
                    context.Movies.AddRange(new List<Movie>()
                    {
                        new Movie()
                        {
                            Name = "2.0",
                            Description = "2.0 is a 2018 Indian Tamil-language 3D science-fantasy action film directed by S. Shankar, co-written with B. Jeyamohan and Madhan Karky.",
                            Price = 399.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/c/cf/2.0_film_poster.jpg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(20),
                            CinemaId = 3,
                            ProducerId = 3,
                            MovieCategory = MovieCategory.Action
                        },
                          new Movie()
                        {
                            Name = "K.G.F: Chapter 2",
                            Description = "K.G.F: Chapter 2 is a 2022 Kannada-language period action film directed by Prashanth Neel, produced by Vijay Kiragandur under Hombale.",
                            Price = 599.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/d/d0/K.G.F_Chapter_2.jpg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(30),
                            CinemaId = 1,
                            ProducerId = 1,
                            MovieCategory = MovieCategory.Action
                        },

                        new Movie()
                        {
                            Name = "Good Newwz",
                            Description = "Good Newwz is a 2019 Indian Hindi-language comedy-drama film directed by Raj Mehta, produced by Dharma Productions and Cape of Good Films.",
                            Price = 299.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/7/76/Good_Newwz_film_poster.jpg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(30),
                            CinemaId = 1,
                            ProducerId = 1,
                            MovieCategory = MovieCategory.Drama
                        },
                        new Movie()
                        {
                            Name = "The Nun",
                            Description = "The Nun (†HE NUИ) is a 2018 American gothic supernatural horror film. Directed by Corin Hardy, written by Gary Dauberman and James Wan.",
                            Price = 199.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/3/34/TheNunPoster.jpg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(70),
                            CinemaId = 4,
                            ProducerId = 4,
                            MovieCategory = MovieCategory.Horror
                        },
                        new Movie()
                        {
                            Name = "14 Peaks",
                            Description = "14 Peaks: Nothing Is Impossible is a gripping 2021 documentary about Nirmal Purja's quest to conquer all 14 world's 8,000-meter peaks.",
                            Price = 399.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/e/ed/14_Peaks-_Nothing_Is_Impossible.jpg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(50),
                            CinemaId = 1,
                            ProducerId = 2,
                            MovieCategory = MovieCategory.Documentary
                        },
                              new Movie()
                        {
                            Name = "The Shawshank Redemption",
                            Description = " 'The Shawshank Redemption' is a timeless 1994 drama film. Directed by Frank Darabont, it follows a man's journey through hope, friendship, and redemption while imprisoned.",
                            Price = 29.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-1.jpeg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(3),
                            CinemaId = 1,
                            ProducerId = 1,
                            MovieCategory = MovieCategory.Drama
                        },
                        new Movie()
                        {
                            Name = "Jawan",
                            Description = "Jawan: Atlee's gripping Hindi action-thriller, Gauri Khan's production under Red Chillies Entertainment, showcases Atlee's Hindi directorial debut.",
                            Price = 399.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/3/39/Jawan_film_poster.jpg",
                            StartDate = DateTime.Now.AddDays(20),
                            EndDate = DateTime.Now.AddDays(90),
                            CinemaId = 1,
                            ProducerId = 3,
                            MovieCategory = MovieCategory.Action
                        },
                        new Movie()
                        {
                            Name = "Mission: Impossible",
                            Description = "Mission: Impossible - Dead Reckoning Part One is a 2023 American spy action film directed by Christopher McQuarrie, co-written with Erik Jendresen.",
                            Price = 399.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/e/ed/Mission-_Impossible_%E2%80%93_Dead_Reckoning_Part_One_poster.jpg",
                            StartDate = DateTime.Now.AddDays(3),
                            EndDate = DateTime.Now.AddDays(20),
                            CinemaId = 1,
                            ProducerId = 5,
                            MovieCategory = MovieCategory.Action
                        },
                          new Movie()
                        {
                            Name = "Pathaan",
                            Description = "Produced by Aditya Chopra of Yash Raj Films, the film began principal photography in November 2020 in Mumbai.",
                            Price = 399.50,
                            ImageURL = "https://th.bing.com/th/id/OIP.nvy7jIzrCQsQrpHpLz5t6wHaNN?w=115&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7",
                            StartDate = DateTime.Now.AddDays(-30),
                            EndDate = DateTime.Now.AddDays(-10),
                            CinemaId = 1,
                            ProducerId = 5,
                            MovieCategory = MovieCategory.Action
                        },  
                        new Movie()
                        {
                            Name = "Pathaan",
                            Description = "Oppenheimer is a 2023 epic biographical thriller film directed by Christopher Nolan, based on the 2005 biography American Prometheus.",
                            Price = 799.50,
                            ImageURL = "https://upload.wikimedia.org/wikipedia/en/4/4a/Oppenheimer_%28film%29.jpg",
                            StartDate = DateTime.Now.AddDays(30),
                            EndDate = DateTime.Now.AddDays(60),
                            CinemaId = 1,
                            ProducerId = 5,
                            MovieCategory = MovieCategory.Documentary
                        },
                    });
                    context.SaveChanges();
                }
                //Actors & Movies
                if (!context.Actors_Movies.Any())
                {
                    context.Actors_Movies.AddRange(new List<Actor_Movie>()
                    {
                        new Actor_Movie()
                        {
                            ActorId = 1,
                            MovieId = 1
                        },
                        new Actor_Movie()
                        {
                            ActorId = 3,
                            MovieId = 1
                        },

                         new Actor_Movie()
                        {
                            ActorId = 1,
                            MovieId = 2
                        },
                         new Actor_Movie()
                        {
                            ActorId = 4,
                            MovieId = 2
                        },

                        new Actor_Movie()
                        {
                            ActorId = 1,
                            MovieId = 3
                        },
                        new Actor_Movie()
                        {
                            ActorId = 2,
                            MovieId = 3
                        },
                        new Actor_Movie()
                        {
                            ActorId = 5,
                            MovieId = 3
                        },


                        new Actor_Movie()
                        {
                            ActorId = 2,
                            MovieId = 4
                        },
                        new Actor_Movie()
                        {
                            ActorId = 3,
                            MovieId = 4
                        },
                        new Actor_Movie()
                        {
                            ActorId = 4,
                            MovieId = 4
                        },


                        new Actor_Movie()
                        {
                            ActorId = 2,
                            MovieId = 5
                        },
                        new Actor_Movie()
                        {
                            ActorId = 3,
                            MovieId = 5
                        },
                        new Actor_Movie()
                        {
                            ActorId = 4,
                            MovieId = 5
                        },
                        new Actor_Movie()
                        {
                            ActorId = 5,
                            MovieId = 5
                        },


                        new Actor_Movie()
                        {
                            ActorId = 3,
                            MovieId = 6
                        },
                        new Actor_Movie()
                        {
                            ActorId = 4,
                            MovieId = 6
                        },
                        new Actor_Movie()
                        {
                            ActorId = 5,
                            MovieId = 6
                        },
                    });
                    context.SaveChanges();
                }
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@etickets.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                adminUserEmail = "Prayash@gmail.com";
                var adminUser2 = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser2 == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Prayash Poudel",
                        UserName = "prayash72",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Prayash@123");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }
    }
}
