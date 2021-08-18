namespace OperaHouseTheater.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;

    using static WebConstants;
    using static Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedPerformanceTypes(services);
            SeedDepartments(services);
            SeedEmployeeCategories(services);

            SeedAdministrator(services);

            //new one:
            SeedPerformance(services);
            SeedNews(services);
            SeedEmployees(services);
            SeedRoles(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();

            data.Database.Migrate();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@oht.com";
                    const string adminPassword = "admin123";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Administrator"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedEmployeeCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();
            
            if (data.EmployeeCategories.Any())
            {
                return;
            }

            data.EmployeeCategories.AddRange(new[]
            {
                new EmployeeCategory{CategoryName = "Примабалерина"},
                new EmployeeCategory{CategoryName = "Премиер-солист"},
                new EmployeeCategory{CategoryName = "Солист"},
                new EmployeeCategory{CategoryName = "Ансамбъл"},
                new EmployeeCategory{CategoryName = "Гост-Солист"},
                new EmployeeCategory{CategoryName = "Сопран"},
                new EmployeeCategory{CategoryName = "Мецосопран"},
                new EmployeeCategory{CategoryName = "Тенор"},
                new EmployeeCategory{CategoryName = "Баритон"},
                new EmployeeCategory{CategoryName = "Бас"},
                new EmployeeCategory{CategoryName = "Директор"},
                new EmployeeCategory{CategoryName = "Художествен ръководител"},
                new EmployeeCategory{CategoryName = "Заместник-директор"},
                new EmployeeCategory{CategoryName = "Главен-художник"},
                new EmployeeCategory{CategoryName = "Главен-диригент"}
            });

            data.SaveChanges();
        }

        public static void SeedDepartments(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();
            
            if (data.Departments.Any())
            {
                return;
            }

            data.Departments.AddRange(new[]
            {
                new Department{ DepartmentName = "Балет"},
                new Department{ DepartmentName = "Опера"},
                new Department{ DepartmentName = "Мениджмънт"},
            });

            data.SaveChanges();
        }

        public static void SeedPerformanceTypes(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();
            
            if (data.PerformanceTypes.Any())
            {
                return;
            }

            data.PerformanceTypes.AddRange(new[]
            {
                new PerformanceType{Type = "Опера" },
                new PerformanceType{Type = "Балет" },
            });

            data.SaveChanges();
        }

        public static void SeedPerformance(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();

            if (data.Performances.Any())
            {
                return;
            }

            data.Performances.AddRange(new[]
            {
                new Performance
                    {Title = "Лебедово езеро",Composer = "П.И.Чайковски",PerformanceTypeId = 2,
                        ImageUrl = "https://theatrenorth.com.au/imager/source-assets/images/events/TN-Subs/468/SwanLake_1200x628px_1d33d8c95f26f8f673c98d6d5bbd0972.jpg",
                    Synopsis = "Първо действие: В замъка празнуват пълнолетието на принц Зигфрид. " +
                    "Забавните танци на Шута се редуват с грациозните танци на девойките и кавалерите им." +
                    " Идва Владетелката, която подарява на сина си арбалет. Тя му съобщава, че на утрешния бал" +
                    " трябва да си избере невеста.Свечерява се. Младежите се разотиват. Зигфрид вижда в мечтите" +
                    " си образа на девойката, в която би могъл да се влюби. Шутът го изтръгва от дълбокия размисъл," +
                    " като му показва прелитащите лебеди. Зигфрид тръгва на лов.",
                    },
                new Performance
                    {Title = "Зорба гъркът",Composer = "Микис Теодоракис",PerformanceTypeId = 2,
                        ImageUrl = "https://operasofia.bg/uploads/events/378.jpg?_=1623914800",
                    Synopsis = "Митът за Зорба винаги е бил приет като символ на човешкия стремеж към свободата." +
                    " За първи път историята е обезсмъртена в новелата на великия гръцки писател Никос Казандзакис, по която е написан " +
                    "сценарий за филма от Михаел Какоянис, а през 1988 година е създадена балетната версия от Лорка Масине по музиката " +
                    "на знаменития гръцки композитор Микис Теодоракис.Балетът Зорба Гъркът, чиято световна премиера се е състояла на 6 август 1988 година " +
                    "на сцената на Арена Ди Верона е мега шоу в две действия и 22 картини. Този спектакъл винаги е имал голям касов успех," +
                    " игран е в над 30 страни и е посетен от над 3 милиона зрители. ",
                    },
                new Performance
                    {Title = "Лешникотрошачката",Composer = "П.И.Чайковски",PerformanceTypeId = 2,
                        ImageUrl = "https://operasofia.bg/uploads/images/2021/02/18963.jpg?_=1613404956",
                    Synopsis = "Това е историята на едно малко момиче, което напразно очаква своя коледен подарък в празничната нощ. И когато получава лешникотрошачка" +
                    " под формата на войник в красива униформа, тя още не знае, че й предстои едно вълшебно пътешествие, в което ще воюва с краля на мишките, че играчката " +
                    "ще се превърне в принц, който ще я отведе при танцуващите снежинки и феи, и накрая ще играе с най- красивите кукли и цветя!",
                    },
                new Performance
                    {Title = "Дон Кихот",Composer = "Лудвиг Минкус",PerformanceTypeId = 2,
                        ImageUrl = "https://i2.wp.com/letsplayoc.com/wp-content/uploads/2018/09/MikhailovskyDonQuixote_615x400.jpg?ssl=1",
                    Synopsis = "Барселона. На площада пред страноприемницата на Лоренцо има веселие. Сред младите хора е дъщеря му - закачливата Китри. " +
                    "Тук е и влюбеният в нея бръснар Базил. Лоренцо не харесва бедния жених. Допада му повече богатия Гамаш, който иска да се ожени за Китри." +
                    " Тълпата приветства уличната тънцьорка Карменсита и тореадора Еспада. Появата на рицаря Дон Кихот и слугата му Санчо Панса предизвиква " +
                    "общо удивление. Дон Кихот е поразен от красотата на Китри, в която вижда прекрасната Дулсинея от бляновете си.",
                    },
                new Performance
                    {Title = "Кармен",Composer = "Жорж Бизе",PerformanceTypeId = 1,
                        ImageUrl = "https://operasofia.bg/uploads/events/312.jpg?_=1593080335",
                    Synopsis = "Музика, текст и театър на движението са базовото триединство на принципите на древногръцката класическа трагедия. " +
                    "Всички жанрове на изкуствата от епохата на Ренесанса безвъзвратно се възраждат от нея.В нашата нова постановка на КАРМЕН от " +
                    "Жорж Бизе режисурата, в погледа от музикалната й драматургия към живота й на сцената, е личен избор за самостоятелен концептуален " +
                    "поглед в стил на смесени принципи от съвременността и традиции от древногръцкия театър. Но и древния",
                    },
                new Performance
                    {Title = "Тоска",Composer = "Джакомо Пучини",PerformanceTypeId = 1,
                        ImageUrl = "https://i.ytimg.com/vi/0zBIEo3WK8c/maxresdefault.jpg",
                    Synopsis = "Първо действие – Църквата Сант’Андреа Дела Вале в Рим.Завесата се вдига под оркестровия грохот " +
                    "на три съкрушителни акорда, музикален мотив, свързан с шефа на полицията, барон Скарпия. Следва втори " +
                    "мотив – неспокойна, низходяща синкопирана тема, която се свързва с преследването на беглеца Чезаре " +
                    "Анджелоти. Той се появява в църквата, изтощен и тръпнещ от страх. Симпатизант на републиканците, той е " +
                    "хвърлен в затвора от роялистите. Бягството му е уредено от сестра му маркиза Атаванти, която му е оставила" +
                    " ключ от семейния параклис и е скрила там женско облекло за дегизиране. Анджелоти намира ключа и се " +
                    "скрива в параклиса.",
                    },
                new Performance
                    {Title = "Травиата",Composer = "Джузепе Верди",PerformanceTypeId = 1,
                        ImageUrl = "https://www.opera-online.com/media/images/avatar/production/3241/xl_avatar.jpg?1531900371",
                    Synopsis = "Музикалните критици определят „Травиата“ като най-дръзката опера на Верди. Не случайно навремето цензурата " +
                    "налага действието да бъде изместено с един век назад, за да бъдат защитени добрите нрави. В действителност композиторът " +
                    "разобличава предразсъдъците на обществото, в което самият той живее.Виолета си проправя пътя в своя свят като куртизанка, но се " +
                    "влюбва в младия благородник Алфред Жермон. Баща му Жорж допуска грешката да я съди, не според човешката й същност, а заради професията й." +
                    " Когато скъсва с Алфредо, тя го прави не само за негово добро, а също и, заради репутацията на семейството му.",
                    },
                new Performance
                    {Title = "Аида",Composer = "Джузепе Верди",PerformanceTypeId = 1,
                        ImageUrl = "https://operasofia.bg/uploads/events/373.jpg?_=1613546539",
                    Synopsis = "Любов или вярност – коя от двете ще надделее във фаталната безизходица?Аида, поробената в Египет етиопска принцеса, " +
                    "е безнадеждно увлечена в своя завоевател  Радамес. В центъра между воюващите народи, тя е изправена пред невъзможен избор. Дали да предаде " +
                    "Радамес, за да защити своята родина?Или да предаде себе си, изцяло на любовта?",
                    },
            });

            data.SaveChanges();
        }

        public static void SeedNews(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();

            if (data.News.Any())
            {
                return;
            }

            data.News.AddRange(new[]
            {
                new News
                {
                    Title = "ОГНЕНА СТРАСТ С БАЛЕТНИТЕ КЛАСИКИ „КАРМЕН“ – СЮИТА И „БОЛЕРО“",NewsImageUrl = "https://i.pinimg.com/originals/b4/3c/8b/b43c8baf52c27ef8c9ec82d441c8f2d3.jpg",
                    Content = "Прочутата творба „Кармен“ – сюита е написана от композитора Родион Шчедрин  и е великолепна интерпретация на гениалната музика" +
                    " на Жорж Бизе.Балетът е създаден специално за голямата руска балерина Мая Плисецкая, хореограф и режисьор е Алберто Алонсо, а премиерата е в" +
                    " Болшой театър през април 1967 г. Преди повече от четиридесет години този спектакъл е показан и на Софийска сцена, при гастрол на Кубинския " +
                    "национален балет. Ролята на Кармен претворява друга голяма балерина – Алисия Алонсо."  
                },
                new News
                {
                    Title = "ОЧАКВАМЕ С НЕТЪРПЕНИЕ ТЕНОРА РАМОН ВАРГАС НА СЦЕНАТА",NewsImageUrl = "https://operasofia.bg/uploads/images/2021/04/19534.jpg?_=1617980699",
                    Content = "В прочутата опера на Верди „Бал с маски“, ще гостува за първи път у нас знаменития  певeц " +
                    "от Мексико, прочут по цял свят – Рамон Варгас . Той ще се превъплъти в ролята на Рикардо, губернатор на" +
                    " Бостън.  Рикардо е благороден, щедър владетел, въпреки, че често е буен и прибързан. Той е тайно влюбен" +
                    " в жената на своят най-близък съветник Ренато. Когато тайната е разкрита, Ренато се включва в заговор срещу живота на Рикардо."
                },
                new News
                {
                    Title = "ПРЕОДОЛЯХ КОРОНАВИРУСА, ВРЪЩАМ СЕ НА СЦЕНАТА!",NewsImageUrl = "https://operasofia.bg/uploads/images/2021/01/18229.jpg?_=1604652343",
                    Content = "Беше тежко и мъчително преживяване. Имаше моменти, когато дишах изключително тежко и трудно. Дробовете ми бяха засегнати, " +
                    "бях развила двустранна пневмония. А за един певец това е голямо изпитание. Дишането е в основата и на пеенето, и на живота, развълнувано" +
                    " споделя Гергана.Певицата трябваше да постъпи в Инфекциозна болница, след като състоянието й се влоши преди време. Сега казва, че е благодарна " +
                    "на лекарите и медицинските сестри. Нарича ги герои, защото са до нея и до всички болни всяка минута. "
                },
                new News
                {
                    Title = "ОТБЕЛЯЗВАМЕ СВЕТОВНИЯ ДЕН НА БАЛЕТА С КОНЦЕРТЪТ",NewsImageUrl = "https://img-cdn.dnes.bg/d/images/photos/0447/0000447805-article2.jpg",
                    Content = "Световният ден на балета, обявен от ЮНЕСКО - 29 април, е жест на признание към най-популярната личност в " +
                    "света на балета Жан-Жорж Новер. Ние, българите, можем да се гордеем, че това решение е взето на сесия през 1982 г. в " +
                    "София.Новер е роден на 29 април 1727 г. и остава в историята на изкуството със своята основополагаща творба Писма върху " +
                    "танца и балета."
                },
            });

            data.SaveChanges();
        }

        public static void SeedEmployees(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();

            if (data.Employees.Any())
            {
                return;
            }

            data.Employees.AddRange(new[]
            {
                new Employee
                {
                    FirstName="Наталия",LastName="Осипова",
                    CategoryId =3 ,DepartmentId = 2, ImageUrl="https://puntomarinero.com/images/natalia-osipova-one-of-the_2.jpg"
                    ,Biography = "Родена в Москва.Наталия Петровна Осипова е руска балерина, в момента главна балерина с The Royal Ballet в Лондон."
                },
                new Employee
                {
                    FirstName="Даниил",LastName="Симкин",
                    CategoryId = 2,DepartmentId = 2, ImageUrl="https://i.pinimg.com/originals/7f/86/fc/7f86fcb59fbfca2db485e0921a31c1a5.jpg"
                    ,Biography = "Даниил Симкин е балетен танцьор с произход от Русия и главен танцьор с Американския театър на балета и Държавния балет в Берлин."
                },
                new Employee
                {
                    FirstName="Иван",LastName="Василиев",
                    CategoryId = 2,DepartmentId =2 , ImageUrl="https://artsmeme.com/wp-content/uploads/2013/07/vasiliev.jpg"
                    ,Biography = "Иван Владимирович Василиев е руски балетист и хореограф. Завършва белорусското балетно училище през 2006г."
                },
                new Employee
                {
                    FirstName="Александрина",LastName="Пендачанска",
                    CategoryId = 6 ,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/252.jpg?_=1590882486"
                    ,Biography = "Пендачанска е родена в София в семейство на известни български музиканти. Започва да свири на пиано на 5-годишна възраст и завършва Националното музикално училище с пиано и пеене."
                },
                new Employee
                {
                    FirstName="Рамон",LastName="Варгас",
                    CategoryId = 8,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/801.jpg?_=1615894848"
                    ,Biography = "Рамон Варгас е сред водещите тенори на нашето време и един от най-търсените по цял свят."
                },
                new Employee
                {
                    FirstName="Атанас",LastName="Младенов",
                    CategoryId = 9,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/209.jpg?_=1590845757"
                    ,Biography = "Атанас Младенов е роден в София в семейство на музиканти. От ранна детска възраст започва да свири на цигулка, а по-късно, в тийнейджърските си години, започва и да пее."
                },
                new Employee
                {
                    FirstName="Гергана",LastName="Русекова",
                    CategoryId = 7,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/194.jpg?_=1590790062"
                    ,Biography = "Гергана Русекова е родена в китния български град Пазарджик. Завършва оперно пеене в ДМА „Панчо Владигеров“, София."
                },
                new Employee
                {
                    FirstName="Цветана",LastName="Бандаловска",
                    CategoryId = 6,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/190.jpg?_=1590788203"
                    ,Biography = "Цветана Бандаловска завършва класическо пеене в Музикалната академия „Панчо Владигеров”."
                },
                new Employee
                {
                    FirstName="Хрисимир",LastName="Дамянов",
                    CategoryId = 8,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/207.jpg?_=1590826568"
                    ,Biography = "Роден през 1987 година в град Шумен. През 2010 година завършва Националната музикална академия „Проф. Панчо Владигеров” в класа на проф. Илка Попова – магистърска степен."
                },
                new Employee
                {
                    FirstName="Александра",LastName="Любчански",
                    CategoryId = 6,DepartmentId = 1, ImageUrl="https://operasofia.bg/uploads/people/251.jpg?_=1590882331"
                    ,Biography = "Александра Любчански завършва следването си по пиано и композиция в Консерваторията в Санкт Петербург, родния й град, дипломирайки се с отличие."
                },
                new Employee
                {
                    FirstName="Екатерина",LastName="Крисанова",
                    CategoryId = 1,DepartmentId = 2, ImageUrl="https://operasofia.bg/uploads/people/124.jpg?_=1590740831"
                    ,Biography = "Родена в Москва, една от големите балетни звезди на Русия.. Обучавана е в балетната школа на Лавровски."
                },
                new Employee
                {
                    FirstName="Денис",LastName="Родкин",
                    CategoryId = 4,DepartmentId = 2, ImageUrl="https://operasofia.bg/uploads/people/120.jpg?_=1590740745"
                    ,Biography = "Денис Родкин е роден в Москва. През 2009 г. завършва хореографското училище при Московския държавен академичен танцов театър."
                },
                new Employee
                {
                    FirstName="Александър",LastName="Волчков",
                    CategoryId = 5,DepartmentId = 2, ImageUrl="https://operasofia.bg/uploads/people/438.jpg?_=1591260484"
                    ,Biography = "Роден в Москва. През 2009 г. завършва хореографското училище при Московския държавен академичен танцов театър."
                },
                new Employee
                {
                    FirstName="Жорж",LastName="Димитров",
                    CategoryId = 15,DepartmentId =3 , ImageUrl="https://operasofia.bg/uploads/people/227.jpg?_=1590874934"
                    ,Biography = "Жорж Димитров е роден през 1979 г.На 4 годишна възраст получава първите си уроци по пиано от проф. Мара Балсамова и Олга Шивачева."
                },
                new Employee
                {
                    FirstName="Стефан",LastName="Господинов",
                    CategoryId = 11,DepartmentId = 3, ImageUrl="https://img-cdn.dnes.bg/d/images/photos/0245/0000245377-fbv.jpg"
                    ,Biography = "Роден в София. Завършва висшето си образование през 2005г."
                },
                new Employee
                {
                    FirstName="Георти",LastName="Георгиев",
                    CategoryId =14 ,DepartmentId = 3, ImageUrl="https://chetilishte.com/wp-content/uploads/2018/08/858587512587%D0%BF.jpg"
                    ,Biography = "Завършва художествената академия. Има много изложби у нас и в чужбина."
                },
            });

            data.SaveChanges();
        }

        public static void SeedRoles(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();

            if (data.RolesPerformance.Any())
            {
                return;
            }

            data.RolesPerformance.AddRange(new[]
            {
                new Role{RoleName = "Принц Зигфрид",PerformanceId = 1},
                new Role{RoleName = "Одета-Одилия",PerformanceId = 1},
                new Role{RoleName = "Ротбарт",PerformanceId = 1},
                new Role{RoleName = "Шут",PerformanceId = 1},
                new Role{RoleName = "Па де троа",PerformanceId = 1},
                new Role{RoleName = "Па де троа",PerformanceId = 1},
                new Role{RoleName = "Па де троа",PerformanceId = 1},
            });

            data.SaveChanges();
        }

    }
}
