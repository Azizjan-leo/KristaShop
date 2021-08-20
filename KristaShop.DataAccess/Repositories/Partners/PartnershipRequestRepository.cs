using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class PartnershipRequestRepository : Repository<PartnershipRequest, Guid>, IPartnershipRequestRepository {
  
        private readonly MySqlCompiler _compiler;

        public PartnershipRequestRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }
        public async Task<IEnumerable<PartnershipRequestSqlView>> GetRequests(bool approved = false) {
            var query = _createPartnershipRequestSqlViewQuery();

            query = approved ? query.WhereTrue("requests.is_confirmed").WhereNotNull("requests.answered_date") : query.WhereFalse("requests.is_confirmed").WhereNull("requests.answered_date");
            
            var compiledSql = _compiler.Compile(query);
            return await Context.Set<PartnershipRequestSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createPartnershipRequestSqlViewQuery() {
            return new Query("part_partnership_requests AS requests")
                        .Select("requests.id", "requests.user_id", "requests.requested_date", "requests.is_accepted_to_process", "requests.is_confirmed", "requests.answered_date")
                        .Select("client.fullname", "client.addresstc", "client.city", "client.phone", "client.email")
                        .SelectRaw("IFNULL(`manager`.`name`, '') AS `manager_name`")
                        .SelectRaw("IFNULL(`1c_city`.`name`, '') AS `city_name`")
                        .SelectRaw("IFNULL(`user_data`.`last_sign_in`, ?) AS `last_sign_in`", DateTimeOffset.MinValue)
                        .Join("1c_clients AS client", "client.id", "requests.user_id")
                        .LeftJoin("1c_manager AS manager", "manager.id", "client.manager")
                        .LeftJoin("1c_city", "1c_city.id", "client.city")
                        .LeftJoin("user_data", "user_data.user_id", "client.id");
        }
    }
}