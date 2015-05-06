using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using SystemTools;
using SystemTools.Extensions;
using SystemTools.WebTools.Infrastructure;
using DataRepository.Exceptions;
using DataRepository.Infrastructure;
using SqlClr;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxorgRepository.Tools
{
    internal class InnerTools
    {
        public const string DefaultSchemaName = "dbo";

        public static bool IsNotSameTaxLoad
        {
            get { return GetIsNotSameTaxLoad(); }
            set { SetIsNotSameTaxLoad(value); }
        }

        public static string AppVersion
        {
            get { return GetAppVersion(); }
        }

        public static bool IsMaintenance
        {
            get { return GetIsMaintenance(); }
            set { SetIsMaintenance(value); }
        }

        private static void SetIsMaintenance(bool value)
        {
            var context = new TaxorgContext(ApplicationCustomizer.ConnectionString);
            context.Database.ExecuteSqlCommand("exec SetMaintenance @value = @p0", value.ToString());
        }

        private static bool GetIsMaintenance()
        {
            var repo = new Repository<Settings>();
            var setting = repo.FirstOrDefault(e => e.Name == Settings.IsMaintenance);
            if (setting == null)
                return false;

            try
            {
                return Boolean.Parse(setting.Value);
            }
            catch (Exception e)
            {
                e.SaveError();
                return false;
            }
        }

        private static string GetAppVersion()
        {
            var repo = new Repository<Settings>();
            var setting = repo.FirstOrDefault(e => e.Name == Settings.AppVersion);
            return setting == null ? "1.0.0.0" : setting.Value;
        }

        private static void SetIsNotSameTaxLoad(bool value)
        {
            var repo = new Repository<Settings>();
            var setting = repo.FirstOrDefault(e => e.Name == Settings.IsNotSameTaxLoad);
            if (setting == null)
            {
                setting = repo.Create();
                setting.Name = Settings.IsNotSameTaxLoad;
                setting.Description = "Запрещает/Разрешает загрузку данных с одинаковыми значениями сумм долга за текущий и предыдущий периоды";
            }

            setting.Value = value ? "1" : "0";
            repo.InsertOrUpdate(setting);
            repo.SaveChanges();
        }

        private static bool GetIsNotSameTaxLoad()
        {
            var repo = new Repository<Settings>();
            var setting = repo.FirstOrDefault(e => e.Name == Settings.IsNotSameTaxLoad);

            return setting != null && setting.Value == "1";
        }

        public static YearMonth GetCurrentPeriod()
        {
            var context = new TaxorgContext(ApplicationCustomizer.ConnectionString);

            try
            {
                return GetCurrentPeriod(context);
            }
            catch (Exception e)
            {
                e.SaveError();
                return new YearMonth(DateTime.Now);
            }
        }

        public static YearMonth GetPrevPeriod()
        {
            return GetCurrentPeriod() - GetPrevPeriodCount();
        }

        private static YearMonth GetCurrentPeriod(DbContext context)
        {
            if (context == null) 
                throw new ArgumentNullException("context");

            var result = ExecScalarFunction<string>(context, DefaultSchemaName, "GetCurrentPeriod");

            if (result == null)
                throw new Exception("Не настроен текущий период");

            return new YearMonth(result);
        }

        public static int GetPrevPeriodCount()
        {
            var context = new TaxorgContext(ApplicationCustomizer.ConnectionString);
            try
            {
                return GetPrevPeriodCount(context);
            }
            catch (Exception e)
            {
                e.SaveError();
                return 1;
            }
        }

        private static int GetPrevPeriodCount(DbContext context)
        {
            return ExecScalarFunction<int>(context, DefaultSchemaName, "GetTaxPrevPeriod");
        }

        public static T ExecScalarFunction<T>(DbContext context, string schemaName, string functionName, params object[] parameters)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            schemaName = functionName.IndexOf('.') > 0 ? String.Empty : schemaName + ".";

            var functionArgs = String.Empty;
            if (parameters.Length != 0)
                functionArgs =
                    parameters.Select((p, index) => String.Format("@p{0}", index.ToString(CultureInfo.InvariantCulture))).Aggregate((s, p) => s + ", " + p);

            var result = context.Database.SqlQuery<T>(String.Format("select {0}{1}({2})", schemaName, functionName, functionArgs), parameters);

            return result.FirstOrDefault();
        }

        public static void SetTaxPrevPeriodCount(int taxPrevPeriod)
        {
            var context = new TaxorgContext(ApplicationCustomizer.ConnectionString);
            context.Database.ExecuteSqlCommand("exec SetTaxPrevPeriodCount @value = @p0", taxPrevPeriod);
        }

        internal static void CheckSaveTaxAccess()
        {
            if (!ApplicationCustomizer.EnableSecurity)
                return;

            var mtOrganization = ContextInfo.ContextInfoCollection[typeof (TaxorgContext)].EntityMetadataCollection[typeof (Organization)];
            var mtTax = ContextInfo.ContextInfoCollection[typeof (TaxorgContext)].EntityMetadataCollection[typeof (Tax)];
            var mtTaxType = ContextInfo.ContextInfoCollection[typeof (TaxorgContext)].EntityMetadataCollection[typeof (TaxType)];

            var access = ApplicationCustomizer.Security.IsAccess(mtOrganization.EntityAlias,
                ApplicationCustomizer.Security.UserName, SecurityAccessType.Insert | SecurityAccessType.Update);
            access &= ApplicationCustomizer.Security.IsAccess(mtTax.EntityAlias, ApplicationCustomizer.Security.UserName,
                SecurityAccessType.Insert | SecurityAccessType.Update);
            access &= ApplicationCustomizer.Security.IsAccess(mtTaxType.EntityAlias, ApplicationCustomizer.Security.UserName,
                SecurityAccessType.Insert | SecurityAccessType.Update);

            if (!access)
                throw new EntityAccessDeniedException(mtOrganization, mtTax, mtTaxType);
        }
    }
}
