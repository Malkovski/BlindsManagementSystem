namespace Blinds.Common
{
    public class GlobalConstants
    {
        public const string AdministratorRoleName = "Admin";
        public const string InitialPassword = "123456";

        // Areas
        public const string AdminAreaName = "Administration";
        public const string AdminControllerName = "Menu";
        public const string AdminActionName = "Index";

        public const string PublicAreaName = "Public";
        public const string ProductsControllerName = "Products";
        public const string OrdersControllerName = "Orders";
        public const string OrdersActionName = "Index";
        public const string MyOrdersActionName = "MyOrders";
        public const string ProductsActionName = "Index";

        // Controllers
        public const string ProjectTitle = "BMS";
        public const string AdministrationControllerTitle = "Администрация";
        public const string RailsControllerTitle = "Релси";
        public const string FabricAndLamensControllerTitle = "Текстил / Ламели";
        public const string ComponentsControllerTitle = "Компоненти";
        public const string PicturesControllerTitle = "Галерия";
        public const string BlindTypesControllerTitle = "Видове щори";
        public const string BlindsControllerTitle = "Щори";

        // User
        public const string Login = "Вход";
        public const string Register = "Регистрация";
        public const string Logoff = "Изход";
        public const string Hello = "Здравей ";
        public const string Manage = "Управление на акаунта";
        public const string Resetpassword = "Рестартиране на парола";
        public const string NewUser = "Нов потребител";
        public const string LoggedInAs = "Логнали сте се като ";
        public const string ChangePassword = "Смяна на парола";
        public const string InvalidLoginAttempt = "Грешно потребителско име или парола";
        public const string CreateNewAccount = "Нов потребител";
        public const string SignInNameText = "Потребител";
        public const string UserNickText = "Потребителско име";
        public const string Password = "Парола";
        public const string CurrentPassword = "Текуща арола";
        public const string NewPassword = "Нова парола";
        public const string RememberMeText = "Запомни ме?";
        public const string UserNameText = "Име";
        public const string UserRepeatPassword = "Потвърдете паролата";
        public const string UserRepeatNewPassword = "Потвърдете новата парола";
        public const string UserPasswordNotmatchErrorMessage = "Паролите не съвпадат!";
        public const string UserAddressText = "Адрес";
        public const string UserLastNameText = "Фамилия";
        public const string UserPasswordLengthErrorMessage = "Паролата трябва да бъде поне {2} символа.";

        // ViewModels

        // Orders
        public const string OrdersName = "Поръчки";
        public const string OrdersDetailsTitle = "ПОРЪЧКА №";
        public const string OrdersDetailsDate = "Заявена на";
        public const string OrdersDetailsOwner = "Контрагент";
        public const string OrdersDetailsInstalation = "Тип монтаж";
        public const string OrdersDetailsShowBlindsButton = "Покажи размерите";
        public const string OrdersDetailsExpeditionDate = "Дата на експедиция";
        public const string OrderNumberRequireText = "Номер е задължителен!";
        public const string OrderNumberExistsMessage = "Вече съществува Ваша поръчка с този номер!";
        public const string OrderAvailableMaterialsErrorMessage = "В момента този продукт не се поддържа!";
        public const string OrderBlindCountErrorMessage = "Броя щори не може да бъде 0 или отрицателен!";
        public const string MakeOrderName = "Поръчай сега!";
        public const string OrderNumberDisplayText = "Номер на поръчката";
        public const string OrderRailDisplayText = "Цвят на релсата";
        public const string OrderColorDisplayText = "Цвят на щората";
        public const string OrderMaterialDisplayText = "Материал";
        public const string OrderTypeDefaultDisplayText = "Изберете вид...";
        public const string MyOrdersTitleText = "Моите поръчки";
        public const string OrderBlindCountText = "Брой щори";
        public const string AddRowsText = "Добави ред";
        public const string CreateNewOrder = "Запиши поръчката";
        public const string AgreeTermsText = "Общите условия!";
        public const string NewOrdersText = "Нова поръчка";
        public const string MyOrdersText = "Моите поръчки";

        // Products
        public const string DownloadInstructionText = "Инструкция за употреба";
        public const string BlindControlSideText = "Управление";
        public const string BlindWidthText = "Хоризонтален размер (в мм.)";
        public const string BlindHeigthText = "Вертикален размер (в мм.)";
        public const string BlindWidth = "Хоризонтал";
        public const string BlindHeigth = "Вертикал";

        // BlindType
        public const string BlindTypeDisplay = "Вид";
        public const string BlindTypeRequireText = "Видът е задължителен!";
        public const string InfoDisplay = "Информация";
        public const string InfoRequireText = "Информацията е задължителна!";
        public const string PriceDisplay = "Цена";
        public const string PriceRequireText = "Цената е задължителна!";
        public const string ContentDisplay = "Изображение";

        // Gallery
        public const string PictureTitleDisplay = "Заглавие";
        public const string PictureTitleRequireText = "Заглавието е задължително!";
        public const string PictureOriginalFileNameDisplay = "Име на файл";
        public const string PictureOriginalSizeDisplay = "Размер";
        public const string PictureExtensionDisplay = "Разширение";

        // Rails
        public const string ColorDisplay = "Цвят";
        public const string ColorRequireText = "Цветът е задължителен!";
        public const string MaterialDisplay = "Материал";
        public const string MaterialRequireText = "Материалът е задължителен!";
        public const string QuantityDisplay = "Количество";
        public const string QuantityRequireText = "Количеството е задължително!";

        // Components
        public const string NameDislply = "Име";
        public const string NameRequireText = "Името е задължително!";
        public const string DefaultAmountRequireText = "Базовото количество за единица щора е задължително!";
        public const string DefaultAmountName = "Базов разход";
        public const string DefaultAmoutWidthBasedName = "W";
        public const string DefaultAmoutHeigthBasedName = "H";

        // Enum
        public const string Red = "Червен";
        public const string Green = "Зелен";
        public const string Blue = "Син";

        public const string GTypeInstall = "Г-образен";
        public const string CTypeInstall = "Таванен";
        public const string SideGuidedTypeInstall = "Стрнично водене";
        public const string BottomFixedTypeInstall = "Фиксатор";

        public const string Plastic = "Пластмаса";
        public const string Aluminium = "Алуминий";
        public const string Wood = "Дърво";
        public const string Screen = "Screen";
        public const string Blackout = "Blackout";
        public const string Transparent = "Полу-прозрачен";

        // Grid
        public const string Send = "Изпрати";
        public const string EditLabel = "Редакция";
        public const string AddEditLabel = "Добави/Редактирай";
        public const string Create = "Нов запис";
        public const string Update = "Обнови";
        public const string Delete = "Изтрий";
        public const string Cancel = "Отказ";
        public const string GroupMessage = "Провлачете заглавие на колона тук, за да групирате по нея";
        public const string FileUploadText = "Избери изображение...";
        public const string ShowDetails = "Преглед";

        // Database constraints
        public const string NameMinLength = "Името трябва да е поне 5 символа!";
        public const string NameMaxLength = "Името не може да е повече от 100 символа!";
        public const string ComponentNameMinLength = "Името трябва да е поне 2 символа!";
        public const string ComponentNameMaxLength = "Името не може да е повече от 150 символа!";
        public const string InfoMinLength = "Описанието трябва да е поне 10 символа!";
        public const string InfoMaxLength = "Описанието не може да е повече от 1500 символа!";
        public const string PriceMinValue = "Цената не може да бъде отрицателна!";
        public const string OrderNumberMaxLength = "Номерът не може да е повече от 40 символа!";
        public const string OrderNumberRegex = "Номерът трябва да съдържа само арабски цифри!";

        // Administration error messages
        public const string BlindTypeExistsMessage = "Типът щора вече съществува!";
        public const string ComponentExistsMessage = "Компонент с това име, за този модел щори, вече съществува!";
        public const string RailExistsMessage = "Вече съществува релса за този вид щори с този цвят!";
        public const string FabricAndLamelsExistsMessage = "Вече съществува текстил/ламели за този вид щори, с този цвят!";

        // General errors
        public const string GeneralDataError = "Грешка с данните";
        public const string GeneralRailError = "Грешка с данните за релси";
        public const string GeneralMaterialError = "Грешка с данните за материал";

        // Others
        public const string AboutUsText = "Blind Manufacture and Supply е създадена през 2015г. като търговска фирма, а година по-късно е подписан лицензен договор с световния лидер в областта на слънцезащитата - фирма Blind Depot. Съгласно договора Blind Manufacture and Supply получава правата да произвежда щори с материали машини и по технологиите на Blind Depot, както и да използва запазените и марки. Blind Manufacture and Supply продава своите продукти чрез собствените си магазини в София, Пловдив, Варна, Бургас и Смолян и в още 40 града чрез 85 представителства. Персоналът на фирмата е високо квалифициран и е преминал специално обучение в Blind Depo. Дългогодишният опит и отдаденост е основа за постигането на висок професионализъм в производството.В резултат на постоянния стремеж на екипа към постигане на най-високо качество при производството на щори, през 2016г.е внедрена система за управление на качеството съгласно международния стандарт ISO 9001:2000. Системата за качесво на Blind Manufacture and Supply е одобрена от международно признатата одиторска компания World Quality Assurance. През 2016г. фирмата е сертифицирана и по международния стандарт за Системи на управление на околната среда ISO 14001:2004 за производство на слънцезащитни прозоречни изделия.";
    }
}
