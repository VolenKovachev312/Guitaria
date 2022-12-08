﻿using Guitaria.Data;
using Guitaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using static Guitaria.Data.Constants.ConstantValues.SeedData;

namespace Guitaria.Infrastrcture
{
    public static class SeedData
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                var testInitializeAccounts = serviceScope.ServiceProvider;
                var data = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                try
                {
                    InitializeAccounts(testInitializeAccounts).Wait();
                    SeedDatabase(data);
                }
                catch (Exception e)
                {
                    ILogger<StartUp> logger = testInitializeAccounts.GetRequiredService<ILogger<StartUp>>();
                    logger.LogError(e, "Couldn't seed database.");
                }
            }

            return app;
        }
        private static async Task InitializeAccounts(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await CreateRoleAsync(roleManager);

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            await CreateAdmin(userManager);
        }

        private static async Task CreateRoleAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            bool adminRoleExists = await roleManager.RoleExistsAsync(AdministratorRole);
            bool userRoleExists = await roleManager.RoleExistsAsync(UserRole);

            if (adminRoleExists || userRoleExists)
            {
                return;
            }
            await roleManager.CreateAsync(new IdentityRole<Guid>(AdministratorRole));
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole));
        }

        private static async Task CreateAdmin(UserManager<User> userManager)
        {
            User testAdmin = await userManager.FindByEmailAsync(AdminEmail);
            if (testAdmin != null)
            {
                return;
            }
            testAdmin = new User()
            {
                UserName = AdminUserName,
                Email = AdminEmail
            };
            await userManager.CreateAsync(testAdmin, AdminPassword);
            await userManager.AddToRoleAsync(testAdmin, AdministratorRole);
        }
        private static void SeedDatabase(ApplicationDbContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }
            List<Category> categories = new List<Category>();
            categories.Add(new Category()
            {
                Name = "Acoustic Guitars",
                ImageUrl = "https://muzikercdn.com/uploads/products/240/24042/main_59242ba1.jpg",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name="LAG Tramontane T70DC Natural Satin",
                        Description="Акустична китара с форма на тялото Дреднаут с разрез. Тялото на китарата е изработено от африкански Sapelle с предна плоча, направена от солиден смърч. Цялото тяло е с матов финиш. Струните се свързат с мост от дърво Brownwood с накрайник от черен графит. Вратът на инструмента е от махагон, също с матов финиш. Както мостта,така и грифа са направени от дърво Brownwood и са снабдени с 20 прагчета тип Medium Jumbo.Мензурата на врата е 650 мм, а ширината на нулевото прагче изработено от графит е 43 мм.Горната част на инструмента е украсена с инкрустация с логото на LAG и от двете страни са поставени три ключа с черно матово покритие. Цвят: Natural(естествен).",
                        ImageUrl="https://muzikercdn.com/uploads/products/369/36916/main_ef8967d6.jpg",
                        Price=580
                    },
                    new Product()
                    {
                        Name="Lava Music Blue Lava Original Freebost White",
                       Description="С тегло само 3,7 lb, китарата е лека за ръцете и раменете. Както и универсалната чанта за китара, която идва с нея. Сега можете да ги вземете и да тръгнете на път с лек бриз. Изработен от твърд HPL материал, BLUE LAVA Original издържа на променяща се температура, влажност и дори на разливане и пръски, предлагайки безгрижно изживяване, където и да играете. Технологията 4-MASS носи на BLUE LAVA Original параметрична структура, проектирана чрез симулиране на динамиката на вибрациите на горната част, тялото, шията и вътрешния въздух, като ги кара да резонират в перфектен унисон за пълен, ярък тон. Вратът на китарата ви позволява да свирите на различни прагове без усилие, благодарение на ергономичната си форма, проектирана според удобните движения на пръстите и тънката пета, предлагаща лесен достъп до по-високите прагове. Прикрепен чрез процес на прецизна обработка, грифът на китарата се характеризира с точно и стабилно действие, подходящо дори за начинаещи. Лесен за премахване и смяна, магнитният капак ви позволява да регулирате релефа на врата без усилие. Технологията FreeBoost използва гърба на китарата като високоговорител, като дава възможност на BLUE LAVA Original да усилва звуците отвътре. Така че всеки път, когато имате нужда от подобрени звуци, докато играете на открито, на събирания или повече, можете да активирате L2 Preamp и да си дадете тласък, без дори да включвате усилвател. Съсредоточен върху идеята за намаляване на влиянието на материала и производството върху природата, BLUE LAVA Original интегрира естествени елементи с модерен дизайн, създавайки уникален стил, който вдъхва живот на всички ваши моменти.",
                        ImageUrl="https://muzikercdn.com/uploads/products/12743/1274368/main_9536d92c.jpg",
                        Price=980
                    },
                    new Product()
                    {
                        Name="Takamine GN30",
                        Description="Акустична китара от серия G30 с оригинална NEX форма на тялото, базирана на умалено Jumbo, която осигурява копринен баланс на тона и поддържа вокалите красиво, което я прави много гъвкав инструмент, който може да покрие много звукова земя. Гърбът и страните на китарата, изработени от махагоново дърво, са комбинирани с горна част от масивен смърч, за да се получи богат, резонансен звук. Тънкият махагонов врат е снабден с гриф от палисандрово дърво с радиус 12\", за да осигури страхотно усещане и възможност за възпроизвеждане, докато синтетичният дизайн на разделена кост на моста от палисандрово дърво без щифтове осигурява превъзходна интонация за по-сладко звучащи акорди и отделни ноти. Инструментът също така разполага със синтетична костна гайка с ширина 42,8 mm, шапка от палисандрово дърво, перлоидна розетка и инкрустации на точки и хромирани тунери, излети под налягане. Дизайн в черен цвят с гланцово покритие.",
                        ImageUrl="https://muzikercdn.com/uploads/products/240/24045/main_ae02e766.jpg",
                        Price=700
                    },
                    new Product()
                    {
                        Name="Takamine GD30-12 Brown Sunburst",
                        Description="GD30-12 е елегантна на вид дредноут 12-струнна китара, която се отличава със солидна конструкция, красив гланц и целия пълен, буен звук, който прави 12-струнен хипнотизиращ. Перфектен за всеки играч, който търси страхотно звучаща 12-струнна китара на достъпна цена, GD30-12 комбинира солидна смърчова плоча с гръб и страни от махагон, за да произведе богат, резонансен звук. Тънкият врат от махагон и 12-инчовият гриф за овангкол осигуряват страхотно усещане и играемост, докато дизайнът на раздвоеното седло на мостчето овангкол осигурява превъзходна интонация за по-сладки звучащи акорди и единични ноти. Други страхотни характеристики включват синтетична костна гайка и мостово седло, капачка за овангкол, перлоидна розетка и инкрустации с точки, хромирани отливки и отлично гланцово покритие.",
                        ImageUrl="https://muzikercdn.com/uploads/products/4927/492763/main_15435265.jpg",
                        Price=740
                    },
                    new Product()
                    {
                        Name="Fender CD-60S WN Mahogany",
                        Description="Акустична китара от серията Fender Classic Design. CD-60S All-Mahogany добавя твърд махагонов връх към един от най-популярните ни модели за отчетливо органичен вкус. Той е идеален за играчи, които търсят висококачествено достъпно dreadnought с чудесен тон и отлична възпроизвеждане. Отличаващ се с новата лесна за игра врата и махагоново гръбче и страни, CD-60S All-Mahogany е идеална за дивана, плажа или кафенето - навсякъде, където искате класическа възпроизвеждане и звук на Fender.",
                        ImageUrl="https://muzikercdn.com/uploads/products/2742/274236/main_e00dcaa8.jpg",
                        Price=389
                    },
                    new Product()
                    {
                        Name="Alvarez MD70BG",
                        Description ="Реакцията на MD70BG е моментална, а бележките му са ясни и стабилни. Палисандърът помага за доставянето на звучни басови честоти, които се допълват от блестящи високи честоти. Когато дрънкате, системата за закрепване FS6 и горната част на AAA Sitka помагат да се осигури яснота и разделяне, а балансът се чува ясно на всичките шест струни. Тези красиво облечени китари отдават почит на традиционните инструменти на Bluegrass с отворени никелови тунери, класически инкрустации, първокласен пикгард в стил на костенурки и безупречно гланцово покритие. Прожекцията и цялостната широчина на тоналния обхват на тази китара са изключителни, което затруднява пренебрегването на тези играчи, които търсят мощен отворен глас и незабавен отговор. Всяка китара е проектирана така, че да извлече най-доброто от своите компоненти и да работят заедно. Тези компоненти включват система за закрепване FS6, двустепенни мостове, истински костни ядки и седла, внимателно подправени тонове и тънко покритие.",
                        ImageUrl="https://muzikercdn.com/uploads/products/8842/884227/main_5d61cdec.jpg",
                        Price=1820
                    },
                     new Product()
                    {
                        Name="Arrow Bronze",
                        Description="Акустична китара от Arrow Бронзова серия в форма на тялото на дредноут. Китара има върха на смърч с гръб и страни от махагон. Китарата се предлага в черен цвят с хромиран хардуер.",
                        ImageUrl="https://muzikercdn.com/uploads/products/3853/385342/main_68a791b1.jpg",
                        Price=245
                    },
                    new Product()
                    {
                        Name="Gretsch G9500 Jim Dandy WN 2-Tone Sunburst",
                        Description="Качество на кракер за кражба!Верен на салоните за китара Gretsch® \"Rex\" от 30-те, 40-те и 50-те години, моделът в салон G9500 Jim Dandy ™ Flat Top олицетворява всичко, което беше страхотно за първата китара на всеки. Всичко и след това, което е, защото G9500 е изработен с подбрани китарни дървета и е напълно облицован и подпрян за топъл и приятен тон, с 24-инчова скала за безкрайни часове на комфорт на свирене.Перфектен както за игра, така и за игра с пръсти, Jim Dandy се отличава с неразрязан корпус от басово дърво с X-скоба за мощен и резонансен глас и орехов мост с горно натоварване с компенсирано синтетично костно седло за равномерна интонация по дължината на врата . Присъединявайки се към тялото на 12 -ия лад, „C“ -формената натрийна врата разполага с удобен, плавно играещ орехов гриф с резки в ретро стил и инкрустации с перлоидни точки. Опакован в класическо двуцветно покритие Sunburst, G9500 също така разполага с ретро стари отворени зъбни колела за настройка, от никелов хардуер и еднослоен бял пикгард с графика „G“. Идеално за писане на песни, практика или дори като китара за пътуване, това малко чудо е чудесно както за начинаещи, така и за опитни професионалисти. Удобен за игра и изпълнен с дървесен винтидж тон с пълнозвучен бас, Jim Dandy ще вдъхнови часове на радост.",
                        ImageUrl="https://muzikercdn.com/uploads/products/3722/372230/main_f8f61ea8.jpg",
                        Price=370
                    },
                    new Product()
                    {
                        Name="Ibanez AAD300CE-LGS",
                        Description="Тяло на Grand Dreadnought. Чрез строг процес на изследователска и развойна дейност в продължение на няколко години, Ibanez преосмисли Dreadnought от нулата, преследвайки висотата на богат звук, здрав нисък клас и богат тон. Тази чисто нова форма на тялото се нарича Grand Dreadnought. Пропорционално, корпусът на Grand Dreadnought е с 5% по-голям от стандартния дредноут, което позволява по-голям обем, по-богат тон и по-мощна реакция от нисък клас. Освен това е изключително удобно за игра, благодарение на обширното внимание към неговата ергономичност.XM Укрепване. Прекарвайки няколко години с опити и грешки, Ibanez най-накрая стигна до нестандартния XM Bracing, за да генерира най-добрия звук, който се откроява на сцената. Внимателно проектираният фестон X-крепеж, разпределението на тонови скоби и скоби за пръсти предават вибрацията на струната ефективно през моста към тялото. XM Bracing създава чист дънен и хрупкав висок звук с подрязан топъл среден клас, но същевременно осигурява широк динамичен обхват и висока реакция. Разширен достъп Неговият дълбок прав отрязък (от 18-ти праг) и обръсната страна позволяват невероятен достъп с високи прагове. Врат с ниско овално захващане Thermo Aged ™ Nyatoh със заоблен гриф. Дръжката на врата с нисък овал е проектирана за лесно свирене на акорди, а заоблените ръбове на грифа осигуряват отлична игра Thermo Aged, специален процес на сушене, прави шията лека и твърда и осигурява стабилност и по-добър резонанс. Функционална конусна глава Новопроектираната форма на главата е резултат от задълбочени изследвания и разследвания. Неговата заострена форма осигурява по-леко тегло, както и право подравняване на струните, което подобрява стабилността на настройката и намалява ефекта на усукване на врата. Ibanez AP11 Магнитен пикап. Пикапът AP11 е малък, лек и позициониран, така че няма да възпрепятства естествените вибрации на горната дървесина. Пикапът генерира силно отзивчив звук с минимална обратна връзка и осигурява оживен отговор в целия честотен диапазон. Звукът е прозрачен и отразява естествения акустичен тон на китарата, като същевременно запазва присъствие и среден клас. AP11 работи добре и с ефекти като реверберация, хор и много други. Вземете контакт. Състои се от три преобразувателя, улесняващи органичната яснота на индивидуалната нота и възпроизвежда естествено ударни техники за потупване на тялото. Този пикап също добавя въздушна дълбочина на тона и яркостта към усиления звук. Инкрустации с луминесцентни странични точки. Луминисцентни инкрустации от странични точки облицоват страничната част на врата за по-лесно навигиране на грифа в условия на слаба осветеност.",
                        ImageUrl="https://muzikercdn.com/uploads/products/5859/585991/main_3b3fb9d6.jpg",
                        Price=1290
                    }
                }
            });
            categories.Add(new Category()
            {
                Name = "Electric Guitars",
                ImageUrl = "https://muzikercdn.com/uploads/products/7268/726880/main_a77ad51e.jpg",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name="ESP LTD Snakebyte Camo",
                        Description="James Hetfield LTD Signature Series Snakebyte се отличава с конструкция на гърлото в мащаб 24,75 инча с корпус от махагон, тънък U-образен махагонов врат, гриф от абанос Macassar, 22 допълнителни джъмбо прага, TonePros заключващ TOM мост и накрайник, и собствените активни пикапи EMG JH SET на Джеймс. Snakebyte Camo се основава на камуфлажна шарка, създадена от високопроизводителния производител на ловни уреди KUIU, дизайн, избран от Hetfield. Snakebyte Camo на LTD включва подходящ специален твърд калъф. Част от приходите от всеки Snakebyte Camo отиват за фондация All Within My Hands на Metallica, благотворителна организация с нестопанска цел, посветена на създаването на устойчиви общности чрез подкрепа на образованието на работната сила, борбата срещу глада и други важни местни услуги.",
                        ImageUrl="https://muzikercdn.com/uploads/products/9347/934730/main_c168e3a6.jpg",
                        Price=4090
                    },
                    new Product()
                    {
                        Name="Jackson JS Series Rhoads Ziricote JS42",
                        Description="Бързи, смъртоносни и достъпни, китарите от серия Jackson® JS правят епичен скок напред, което улеснява от всякога получаването на класически звук на Jackson, външен вид и възпроизвеждане, без да нарушавате банката. Серията JS Rhoads Ziricote JS42 се отличава с изместен ъглов корпус от махагон с горна част от цирикот и закрепена с болтове карамелизирана кленова скоростна шийка с графитна армировка за стабилност като скала и шарфова връзка за намалена умора от игра. Помещавайки 24 джъмбо прага и черни инкрустации на перки на акула, карамелизираният гриф с радиус 12”-16” създава идеалната повърхност за игра за дебели рифове и лесно акордиране близо до нюта, докато постепенно се изравнява в горните регистри за светкавични сола и широки завои без страх от безпокойство. Други характеристики включват двойни хъмбъкинг пикапи Jackson с висока производителност с керамични магнити за вулканичен тон, трипосочен превключвател на пикапа, единични контроли за силата на звука и тона и двойно заключваща се тремоло система на Jackson Floyd Rose® за динамична реакция и надеждна стабилност на настройката . Тази лъскава, ъглова брадва се предлага в ново естествено покритие със златен пикгард, черен заострен гриф и златен хардуер.",
                        ImageUrl="https://muzikercdn.com/uploads/products/9341/934188/main_44258a86.jpg",
                        Price=920
                    },
                    new Product()
                    {
                        Name="ESP LTD Arrow-401",
                       Description="Напълно уникален в смелия си дизайн, LTD ARROW-401 е очевиден избор за метал, хард рок и други жанрове, където е добре да се открояват от тълпата. Този звук на китара е толкова агресивен, колкото изглежда, прецизно изработен с дизайн през врата и комбинация от убийци на активни микрофони EMG 81/85. Включени са висококачествени тунери Grover, махагоново тяло, 3 бр. кленово гърло с 24 екстра джъмбо с фрезова дървесина за най-бързите ви рифове и най-бързите ритми.",
                        ImageUrl="https://muzikercdn.com/uploads/products/504/50474/main_e20b18d8.jpg",
                        Price=2000
                    },
                    new Product()
                    {
                        Name="Epiphone Flying V Prophecy Yellow Tiger Aged Gloss",
                        Description="Modern Collection",
                        ImageUrl="https://muzikercdn.com/uploads/products/5828/582833/main_626bbc21.jpg",
                        Price=1560
                    },
                    new Product()
                    {
                        Name="Epiphone Extura Prophecy Black Aged Gloss",
                        Description="Modern Collection",
                        ImageUrl="https://muzikercdn.com/uploads/products/5828/582803/main_c76987be.jpg",
                        Price=1092
                    }
                }
            });
            categories.Add(new Category()
            {
                Name = "Electro-Acoustic Guitars",
                ImageUrl = "https://musicdivision.bg/wp-content/uploads/2021/12/VNT-VE440WK.jpeg",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name="Takamine GD30CE",
                        Description="...",
                        ImageUrl="https://muzikercdn.com/uploads/products/239/23984/main_d09b12ff.jpg",
                        Price=960
                    },
                    new Product()
                    {
                        Name="Fender Squier SA-105CE",
                        Description="Електроакустична китара Dreadnought с изрезка, ламиниран смърчов плот, ламиниран нато гръб &amp; страни, нато врата и гриф от палисандрово дърво с 20 средни джъмбо прага. Китарата е оборудвана с мост от палисандрово дърво с компенсирано седло, отляти под налягане тунери, 643 мм скала, ширина на гайката 43 мм и Fender пикап система.",
                        ImageUrl="https://muzikercdn.com/uploads/products/383/38345/main_cf0b8028.jpg",
                        Price=262
                    },
                    new Product()
                    {
                        Name="Yamaha LL-TA VT Vintage Tint",
                        Description="Транс-акустична китара от серията TA с Yamaha Original Jumbo тип тяло. Няма нищо толкова вдъхновяващо като свиренето на китара в една чудесна стая - това ви кара да играете по-добре, по-дълго и с повече креативност. Yamaha TransAcoustic Guitar пресъздава това невероятно преживяване, без да се нуждае от никакво външно усилване или ефекти, а само самата китара. Това е най-вдъхновяващата, ангажираща акустична китара, която някога сте свирили. - Масаж от махагон / палисандрово дърво 5см - Мащаб: 650 мм - Фланец: Ебен - Брой на фрезите: 20 - Радиус на шийката: 400 мм - Ебенбов мост - Ширина на гайката : 44 мм - Електроника: SYSTEM70 TransAcoustic + пиезо пикпой - завършване: Vintage Tint.",
                        ImageUrl="https://muzikercdn.com/uploads/products/408/40875/main_5603d71b.jpg",
                        Price=2150
                    },
                    new Product()
                    {
                        Name="Fender Player Series Acoustasonic Telecaster",
                        Description="Еволюцията на революционната платформа Acoustasonic на Fender продължава с представянето на Acoustasonic Player Telecaster. Тази акустично-електрическа китара предлага собствен набор от шест гласа, който демонстрира нейната уникална личност. Опростеният 3-посочен превключвател позволява смяна на формата между акустични и електрически тонове. С Acoustasonic Player Telecaster тази платформа е усъвършенствана, за да предложи универсален, по-достъпен инструмент за съвременния музикант.",
                        ImageUrl="https://muzikercdn.com/uploads/products/8996/899608/main_30697942.jpg",
                        Price=2110
                    },
                    new Product()
                    {
                        Name="Yamaha SLG200N",
                        Description="Електро-акустична китара от серията SILENT. Тялото на китарата е изработено от махагон, тялото е изработено от розово дърво и клен, вратата е изработена от махагон. Фалцът е направен от палисандрово дърво, гайката е с ширина 50 мм.",
                        ImageUrl="https://muzikercdn.com/uploads/products/330/33072/main_25ecf296.jpg",
                        Price=1500
                    }
                }
            });
            categories.Add(new Category()
            {
                Name = "Bass Guitars",
                ImageUrl = "https://musicdivision.bg/wp-content/uploads/2021/12/VNT-V964BLK.jpeg",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name="SX SPB57 2-Tone Sunburst",
                       Description="Електрическа бас китара Vintage PB 57 series. Три тона Sunburst. Корпус от 3 части в традиционен ретро стил, изработен от масивна липа. Страхотен звук, идеален за рок, джаз, блус и кънтри! Кленов врат с гриф от канадски клен, 20 прага и точкови инкрустации. 2x пикапи с единична намотка.",
                        ImageUrl="https://muzikercdn.com/uploads/products/150/15050/main_5b84196c.png",
                        Price=350
                    },
                    new Product()
                    {
                        Name="ESP LTD JC-4 John Campbell Dark Grey Metallic Satin",
                        Description="Като член-основател на Lamb of God, Джон Кембъл е източникът на мощния нисък клас за тази известна американска метъл група от 1994 г. насам. Неговият бас LTD Signature Series JC-4 предлага цялото качество, усещане за свирене и тон, които вие Ще са необходими за всякакъв вид музика, където басът е от съществено значение. Въз основа на формата на корпуса ESP Stream, JC-4 има конструкция с болтове в 34-инчов мащаб, използвайки корпус от блатна пепел и шийка от пет части от клен/лилаво сърце с бързо свирещ тънък U контур на врата. Абаносовият гриф Macassar включва 21 екстра-джъмбо прага, инкрустации с разделени блокове и светещи в тъмното странични маркери. Този добре балансиран бас включва компоненти от висок клас като Hipshot Ultralight тунери и Hipshot A Style мост с прорези. Звукът на JC-4 се захранва от набор активни пикапи Fishman SB-1. Това са многофункционални многогласови пикапи, които предлагат гъвкаво формиране на тона с отличен динамичен диапазон и артикулация. Неговите контроли включват сила на звука с натискане и издърпване за включване на режим на единична намотка, 2-лентов еквалайзер с подредени баси/високи честоти и 3-посочен мини превключвател за избор между три звука на пикапа.",
                        ImageUrl="https://muzikercdn.com/uploads/products/11735/1173575/main_db1c81bb.jpg",
                        Price=4890
                    },
                    new Product()
                    {
                        Name="Sire Marcus Miller V9 Swamp Ash-5 2nd Gen",
                        Description="5-струнна бас китара от серия Vire Marcus Miller V9 (2-ро поколение). Маркус Милър и Сир са били на мисия през последните няколко години, за да изградят най-доброто качество на баса на невероятна цена, което го прави достъпно за всички. Резултатът е нова линия бас модели на Marcus Miller, които имат невероятен звук, фантастичен нов облик и са с високо качество, но с изключително ниска цена. Типът джаз бас е стандартният басов модел в индустрията със своя оригинален звук. Това е бас, който е подходящ за свирене, плесване и други техники и стилове на свирене. С едновременно класическо и модерно усещане и дизайн, този нов бас може да произведе желания звук за всички музикални жанрове от мек до дори силен метален гръндж.",
                        ImageUrl="https://muzikercdn.com/uploads/products/2400/240024/main_08ad2730.jpg",
                        Price=1740
                    }

                }
            });
            categories.Add(new Category()
            {
                Name = "Amplifiers",
                ImageUrl = "https://guitar.com/wp-content/uploads/2020/05/Fender-68-Custom-Pro-Reverb-Credit-Fender@1400x1050.jpg",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name="Boss Katana 50 MKII",
                        Description="Katana MkII извежда популярната серия усилватели за китара на Katana на следващото ниво, като турбо зарежда основната платформа с повече звуци, повече ефекти и повече функции. Сега са налични новозвучни варианти за всичките пет усилвателни знака, удвояващи опциите за тоналност. А категориите за бордови ефекти са разширени от три на пет, което осигурява още повече обработка в реално време, за да избирате. Използваемостта е подобрена и с входен усилвател за модели и мулти-FX, актуализиран софтуер за редактор и много други подобрения.",
                        ImageUrl="https://muzikercdn.com/uploads/products/3176/317610/main_de8773f6.jpg",
                        Price=560
                    },
                    new Product()
                    {
                        Name="Orange Crush 12",
                        Description="2-канален транзистор китарен усилвател от серията Crush предлага типичeн звук. Той разполага с 3-лентов еквалайзер, предусилвател чувствителен overdrive и High Gain,наситен звук или overdrive канали, подобен на този на култовия Rockerverb апарат под формата на един транзистор.",
                        ImageUrl="https://muzikercdn.com/uploads/products/329/32927/main_4e8a7e73.jpg",
                        Price=245
                    },
                    new Product()
                    {
                        Name="Marshall MG15G",
                        Description="Компактни усилватели 15w, които пакетират много енергия. 8 / 8Ohm / потребителски високоговорител осигурява чудесен звук за тренировка, но може да се задържи и пред малка тълпа. Тези усилватели дават допълнителен удар и долен край на звука ви. Разнообразието от тонални опции със сигурност ще допълни всеки стил. С два канала, чист и претоварен, както и три лентов EQ, има тон за всеки. Този компактен усилвател е чудесен размер не само за домашна употреба, но и за практикуване на банда. Чувствайте се като рок бог, използвайки MP3 / Line, за да играете заедно с любимите си песни и емулирания изход за слушалки за изключителна мълчалива практика. Изходи: Изход за слушалки с 1 х 3,5 мм жак. Входове: 1 x 1/4 инчов инструмент за вход, 1 x 3.5 мм Aux инч. Контроли: Чист обем, бутон за избор на канал, Overdrive, Gain, Overdrive звук, Bass, Middle, Treble. Ширина: 375 мм. Височина: 370 мм. Дълбочина: 195 мм. Тегло: 7.6 кг.",
                        ImageUrl="https://muzikercdn.com/uploads/products/650/65016/main_47b08607.jpg",
                        Price=205
                    },
                    new Product()
                    {
                        Name="AER Alpha",
                        Description="Компактен комбо, предназначен за електроакустични инструменти, предлагащ страхотна тонова проекция. Снабден е с един канал с два отделни входа, благодарение на което е възможно паралелно свързване на инструмента и микрофона. Той има мощност 40W при импеданс 4Ohm и един 8 \"високоговорител. Освен това е снабден с вградена реверберация, конектори за външен контур за ефекти и изход за слушалки.",
                        ImageUrl="https://muzikercdn.com/uploads/products/3118/311808/main_9c28427f.jpg",
                        Price=2100
                    }
                }
            });
            categories.Add(new Category()
            {
                Name = "Accessories",
                ImageUrl = "https://m.media-amazon.com/images/I/715-5YDR+7L.jpg",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name="CNB CB1880D Калъф за акустична китара",
                        Description="Качествена джага с 20 мм подложка за китара dreadnought в сив дизайн, изработен от плътна материя. Характеризира се с висока издръжливост и дълъг живот, което се осигурява от качествени ципове YKK. Съдържа няколко отделения за аксесоари, мека плюшена вътрешна част, която защитава вашия инструмент, подложка за врата на китара с велкро, удобни ергономични презрамки за гърба и две дръжки за ръце.",
                        ImageUrl="https://muzikercdn.com/uploads/products/8079/807954/main_bda1c540.jpg",
                        Price=121
                    },
                    new Product()
                    {
                        Name="D'Addario NYXL1052",
                        Description="Хибридно леко горно / тежко дъно 6 комплект за електрическа китара от серията NYXL, което осигурява мощен край за по-силно оформяне, по-голяма якост и до 131% по-голяма стабилност на настройката. Струните са изработени изцяло от сплав от висококачествена въглеродна стомана, произведена в Ню Йорк, и използват изцяло нов метод за изтегляне на тел, съчетан с революционен процес на \"синтез на синтез\" за обикновените стомани. Преобразуваните никелирани нишкови намотки имат по-големи магнитни свойства, което води до по-висока производителност и подобрена честотна характеристика на средния обхват за повече присъствие и криза. Опаковката на комплекта е екологично чиста и устойчива на корозия, така че запазва нишките винаги свежи. D'Addario струните са направени в САЩ Измерители: .010, .013, .017, .030w, .042w, .052w.",
                        ImageUrl="https://muzikercdn.com/uploads/products/279/27967/main_90a99525.jpg",
                        Price=37
                    },
                    new Product()
                    {
                        Name="Dunlop 6501",
                        Description="Комплект продукти за полиране на китара и пръстен, който съдържа два популярни полиуретаи Dunlop: Формула No. 65 Polish and Cleaner liquid, и Bodygloss 65 Cream of Carnauba wax. Формулата 65 Polish and Cleaner е стандартна почистваща препарат за ежедневна употреба, която създава истински блясък без остатъци и има подходящ химически баланс за запазване на всяко покритие. Bodygloss Cream of Carnauba е най-висококачественият восък, който полира и украсява покритието, като същевременно крие леки драскотини и оставя влага и устойчива на петна бариера. Set също съдържа 100% памучен полиращ плат и инструкции за грижа. Обем на двата продукта: 118 мл (4 унции).",
                        ImageUrl="https://muzikercdn.com/uploads/products/2998/299866/main_64d6d20d.jpg",
                        Price=15
                    },
                    new Product()
                    {
                        Name="TC Electronic Polytune 3 Mini",
                        Description="Малък полифоничен тунер с множество режими на настройка и вграден Bonafide буфер. С POLYTUNE наследство, POLYTUNE 3 MINI предлага безпрецедентно количество мощност за настройка в малък отпечатък, без да се жертват съществените функции, като винаги включен режим и нашата аудиофилна схема BONAFIDE BUFFER за безкомпромисни тонове - което я прави идеална за напълно натоварени педални дъски където всеки инч е от значение!Дали вашият сигнал отчаяно се нуждае от силна буферна верига, за да запазите тоналността ви от взвод на гладни истински байпасни педали? POLYTUNE 3 MINI ви покрива за всяка настройка, а след това и за някои. Благодарение на вградения BONAFIDE BUFFER, който осигурява нула деградация на сигнала и изключително високо съотношение сигнал / шум, POLYTUNE 3 MINI е най-добрият 2-за-1 джобен инструмент за педалите. Изключете буфера и веднага ще се върнете в режим на истински байпас.",
                        ImageUrl="https://muzikercdn.com/uploads/products/2625/262578/main_95afe3a1.jpg",
                        Price=184
                    },
                    new Product()
                    {
                        Name="Cherub WSM-330-TB Механичен метроном",
                        Description="Това е традиционен механичен метроном с висока точност във формата на кула с прецизен пружинен механизъм. Променлив темп от 40 до 208 bpm с регулируем звънец, който може да бъде настроен на 2, 3, 4 или 6 удара. Стилен и отличителен традиционен механичен метроном.",
                        ImageUrl="https://muzikercdn.com/uploads/products/3680/368027/main_75413f28.jpg",
                        Price=74
                    }
                }
            });
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
