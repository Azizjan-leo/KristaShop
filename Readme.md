## Установка настроек Visual Studio и Resharper
В проекте используется не дефолтное форматирование кода, поэтому перед работой необходимо импортировать следующие настройки:
* **KristaShop.sln.DotSettings** - настройки resharper
* **KristaShop.sln.vssettings** - настройки visual studio

Для того чтобы импортировать файл ```KristaShop.sln.DotSettings``` нужно перейти ```Extensions => Resharper => Options``` в открывшимся окне
в левом нижнем углу нажать на кнопку **Manage**. Откроится окно настроек, нажать на зеленую кнопку "**+**" напротив пункта Solution.KristaShop.Personal
(при желании можно выбрать любой другой пункт), затем нажать **Open settings file** и задать путь к файлу ```KristaShop.sln.DotSettings```
который находится в корне проекта

Для того чтобы импортировать файл ```KristaShop.sln.vssettings``` нужно перейти по пути ```Tools => Import and Export settings```
в открывшимся окне выбрать **Import selected environment settings** после чего нажать **next** 2 раза, затем **browse** и задать путь к файлу
```KristaShop.sln.vssettings``` который находится в корне проекта, затем нажать **next** и **finish**

## KRISTA SHOP SITE
На сервере **new.krista.fashion** проект лежит в папке ```/var/www/krista```
Для его запуска/остановки на сервере нужно выполнить команды:

* systemctl start krista.service
* systemctl stop krista.service

При деплое приложения содержимое папки ```/var/www/krista``` можно полностью удалять
Все файлы которые загружаются через приложения лежат в папке ```/var/www/krista-files```, папка **krista-files** содержит следующие папки:

* **colors** - изображения цветов моделей, она синхронизируется с серверером на котором стоит файловый сервис
* **files** - файлы прикрепленные к документам, здесь кешируются скачанные с файлового сервиса файлы документов
* **cache** - в данной папке кэшируются все изображения которые проходят через resize middleware приложения
* **galleryphotos** - здесь храняться все изображения которые загружаются через приложение
* **fileserver** - здесь храняться все изображения которые загружаются в хранилище wysiswyg редактора админки

Поскольку данные папки с файлами лежат не в проекте, для корректной работыы приложения, к ним настроены **location пути в nginx**:

```
location /galleryphotos {
 if ($is_args = "") {
  root /var/www/krista-files;
  break;
 }
 if ($is_args != "") {
  proxy_pass http://localhost:5000;
 }
}

location /colors {
 root /var/www/krista-files;
}

location /fileserver {
root /var/www/krista-files;
}
```

При запуске приложения в Debug по умолчанию используется конфиг ```appsettings.Development.json```,
перед запуском локально необходимо прописать UserSecrets в файле ```secrets.json```,
```
{
  "ConnectionStrings": {
    "KristaShopMysql": ""
  },
  "GlobalSettings": {
    "FilesDirectoryPath": "" //полный путь к wwwroot папке проекта
  }
}
```

---

## Переменные окружения

Конфиги приложений лежат в файлах ```appsettings.Development.json``` и ```appsettings.Production.json```
какой конфиг использовать **выбирается в зависимости от указанных переменных окружения**
для того чтобы задать environment переменную для всей машины нужно в **файле ```/etc/environment``` прописать**:

* ASPNETCORE_ENVIRONMENT=Production - для продакшн сервера
* ASPNETCORE_ENVIRONMENT=Development - для девелоп сервера

*если ASPNETCORE_ENVIRONMENT не сработает, то нужно прописать DOTNET_ENVIRONMENT, с такими же значениями*

Также, переменные **можно прописать в самом сервисе krista.service**, это нужно для того, чтобы переменная 
окружения была доступна только на уровне данного сервиса а не всей машины это делается следующим образом:

* ENVIRONMENT=ASPNETCORE_ENVIRONMENT=Production

---

## МИГРАЦИИ

Для выполнения миграций нужно стянуть проект с репозитория на сервер (сделать git clone если проекта еще нет на сервере)
На сервере **исходники** проекта лежат в папке ```/var/www/repository/krista_web_api```, нужно перейти в нее и выполнить команду **git pull**
Чтобы выполнить миграцию нужно перейти в папку ```var/www/repository/KristaShop.DataAccess``` и выполнить команду:

```
dotnet ef database update -s ../KristaShop.WebUI -c KristaShopDbContext 
```

Для того чтобы миграция выполнилась корретно нужно чтобы **на уровне машины** стояла **переменная окружения**
```ASPNETCORE_ENVIRONMENT=Production``` (Или DOTNET_ENVIRONMENT=Production), это нужно потому, что при выполнении миграций нужно подключаться к БД и для этого нужно подтянуть правильный конфиг.

---

## DEPLOY

Для деплоя нужно выполнить **publish** проекта, это можно сделать из Visual Studio
Правой кнопкой мыши по KristaShop.WebUI => выбрать Publish
Выбрать публиковать в папку (Folder profile) **с настройками**:

* Configuration: release
* Target framework: netcoreapp3.1
* Target runtime: linux-x64

после нажать Publish. Проект опубликуется в папку ```KristaShop.WebUI\bin\Release\netcoreapp3.1\publish```
содержимое данной папки необходимо **заархивировать в .zip** архив (например, с названием publish.zip)
перенести архив через SFTP (например, через Filezilla) на сервер в папку ```/var/www/krista```
на сервере перейти в эту папку и выполнить команду ```unzip -o publish.zip```

Перед выполнением этой команды нужно остановить сервис приложения ```systemctl stop krista.service```
После выполнения запустить сервис приложения ```systemctl start krista.service```

Для выполнения publish из командной строки, находять в папке с исходным кодом проекта можно выполнить данную команду:
```
dotnet publish KristaShop.WebUI --configuration Release --runtime linux-x64 --framework netcoreapp3.1
```

---

## Доступ к репозиторию на сервере

Для того **чтобы стянуть исходный код** проекта на сервер new.krista.fashion через git **НЕ НУЖНО логиниться** со своего аккаунта. Там реализован доступ по ключу [Bitbucket Access Key](https://confluence.atlassian.com/bitbucketserver/ssh-access-keys-for-system-use-776639781.html)
который дает только **readonly доступ** к репозиторию, т.е. с сервера нельзя делать push в репозиторий. Private ключ лежит на сервере в папке ```/home/kpalych/.ssh```, а public ключ лежит в настройках репозитория в разделе Access Keys.

---

## Структура solution (решения)

Solution состоит из следующих проектов:

* **KristaShop.WebUI** - UI и админка приложения
* **KristaShop.Business** - Бизнес логика приложения
* **KristaShop.Common** - Общие классы проекты/хелперы
* **KristaShop.DataAccess** - Контекст доступа к БД krista_shop


## CSRF защита
В новых версиях .net core CSRF защиту, которую использует AntiForgeryToken нужно подключать вручную в методе ConfigureServices в Startup.cs, из коробки она работает не всегда. Для работы защиты необходимо сохранять ключи. В KristaShop.WebUI ключи сохраняются в директорию на диске и шифруются с помощью pfx сертификата. В appsettings.json есть раздел DataProtection, где указываются такие настройки как:

* **KeysDirectory** - путь к папке, куда будут сохраняться ключи
* **CertificatePath** - путь к сертификату, по которому будут шифроваться ключи
* **EnvironmentVariableWithCertificatePassword** - название переменной окружения в которой будет храниться пароль от сертификата
```
 "DataProtection": {
    "KeysDirectory": "/folder",
    "CertificatePath": "/folder/certificate.pfx",
    "EnvironmentVariableWithCertificatePassword": "VARIABLE_NAME"
  },
```

***

## Создание модулей

Приложение разделено на модули. Для создания нового модуля необходимо создать новый проект "Razor Class Library" (если создаете через visual studio, нужно нажать на галочку Add controllers and views на экране создания приложения).

Для корректной работы модуля файл csproj модуля должен содержать следующие настройки:
```
<Project Sdk="Microsoft.NET.Sdk.Razor">

    <PropertyGroup>
        <TargetFramework>net5.0</TargetFramework>
        <AddRazorSupportForMvc>true</AddRazorSupportForMvc>
    </PropertyGroup>

    <ItemGroup>
        <FrameworkReference Include="Microsoft.AspNetCore.App" />
        <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="5.0.6" />
        <PackageReference Include="Microsoft.AspNetCore.StaticFiles" Version="2.2.0" />
    </ItemGroup>

    <PropertyGroup>
        <StaticWebAssetBasePath Condition="$(StaticWebAssetBasePath) == ''">YourModuleName</StaticWebAssetBasePath>
    </PropertyGroup>
</Project>

```
* **AddRazorSupportForMvc** - нужен для того, чтобы корректно работали Razor Views.
* **Ссылки на пакеты** - ссылки на `Microsoft.Extensions.FileProviders.Embedded` и `Microsoft.AspNetCore.StaticFiles` нужены для того, чтобы корректно работал StaticWebAssetBasePath.
* **StaticWebAssetBasePath** - Префикс для доступа к статик файлам модулей. Пример: /YourModuleName/your-file.js. По умолчанию Razor Class Library создает wwwroot папку, для доступа к статик файлам в этой папке применяется следующий систаксис: `_content/YourModuleName/your-file.js`. Т.е. если вы создали модуль под названием "Krista.TestModule" и в wwwroot папке содержится файл с названием test-script.js, то для того чтобы получить доступ к нему нужно перейти по пути: _content/Krista.TestModule/test-script.js. Для того чтобы избавиться от префикса _content и указания полного имени проекта, используется StaticWebAssetBasePath. Если вы не хотите, чтобы у модуля был какой-либо префикс, нужно указать: ```<StaticWebAssetBasePath Condition="$(StaticWebAssetBasePath) == ''">/</StaticWebAssetBasePath>```. Таким образом файлы будут доступны по обычному пути, как если бы они лежали не в модуле, а в обычном wwwroot.

### Area в модулях

Если модуль представляет собой Area, то `все controller'ы и их view нужно ложить в папку имя которой - имя вашей area`. Данная папка должна лежать в корневом каталоге модуля. Например если есть area - 'Admin', то структура должна быть следующая: YourModule/Admin/Controllers, YourModule/Admin/Views. Также любую area можно положить в папку Areas внутри модуля. Но рекомендуется использовать первый подход.

Для того чтобы такой подход работал, в startup классе в методе ConfigureServices нужно прописать следующее:
```
 services.Configure<RazorViewEngineOptions>(x => x.ViewLocationExpanders.Add(new ModulesViewLocationExpander()));
```
Это нужно для того, чтобы razor engine мог находить views в корне приложения по заданной area.

Класс ModulesViewLocationExpander кастомный, он выглядит следующим образом:
```
 public class ModulesViewLocationExpander : IViewLocationExpander {
        public void PopulateValues(ViewLocationExpanderContext context) {
            
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations) {
            return viewLocations.Concat(new[] {
                "/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension,
                "/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension,
            });
        }
    }
```

При такой конфигуции Visual Studio перестанет находить views и к ним не возможно будет перейти из контроллера. Исправить это в visual studio невозможно. Но для Resharper/Rider достаточно прописать:
```
using JetBrains.Annotations;

[assembly: AspMvcViewLocationFormat("~/Admin/Views/{1}/{0}.cshtml")]
[assembly: AspMvcViewLocationFormat("~/Admin/Views/Shared/{0}.cshtml")]
```
Эти атрибуты можно добавить в любой класс внутри проекта, однако для чистоты кода рекомендуется `добавлять` их `в пустой класс` с названием `AssemblyAttributes`.

### Подключение модулей

**`В KristaShop.WebUI обязательно нуно добавлять ссылку на проект модуля через AddReference`**
Модули **подключаются в класс StartUp** внутри метода **ConfigureServices** в Web Application проекте. Для корректного подключения **нужно вызвать 2 метода**:
 * `AddApplicationModules()` который в качетсве аргумента принимает массив assembly модулей, он должен вызываться после метода `AddControllersWithViews()` или после метода `AddMvc()`
 * `ConfigureModulesRuntimeCompilation()` - который в качестве первого агрумента примимает Environment.ContentRootPath, а в качетсве второго аргумента принимает массив assembly модулей, тот же массив что принимает `AddApplicationModules()`. Метод вызывается на объекте IServiceCollection типа. **должен использоваться только в Development Environment**
 * Методы должны вызываться по порядку, сначала: `AddApplicationModules()` а после `ConfigureModulesRuntimeCompilation()`
 * Реализация данных методов находится в файле KristaShop.WebUI.Infrastructure.ModulesConfiguration

В проекте `KristaShop.WebUI` данное подключение находится в файле `KristaShop.WebUI.InfrastructureMvcConfiguration`.

`KristaShop.WebUI` содержит `класс ModulesDeclaration` в конструкторе которого прописываются неймспейсы всех модулей которые необходимо включить в проект.

##### Пример 

Пример подключения модуля "Module.Example". "Module.Example" - неймспейс модуля (т.е. название проекта)
```
    services.AddControllersWithViews()
        .AddApplicationModules(Assembly.Load("Module.Example"))
        .AddRazorRuntimeCompilation();
    
    services.ConfigureModulesRuntimeCompilation(environment.ContentRootPath, Assembly.Load("Module.Example"));
```

##### Примечание

**Для корректной работы [ImageSharp.Web](https://docs.sixlabors.com/articles/imagesharp.web/gettingstarted.html) в Development Environment**, класс **CustomPhysicalFileSystemProvider использует список модулей, чтобы получить** пути ко всем папкам **wwwroot во всех модулях** приложения. Если этого не будет, ImageSharp не будет работать с файлами из wwwroot папки других модулей. В других Environment'ах это не нужно потому, что после publish'а проекта, wwwroot папки всех модулей собираются в одну.

***