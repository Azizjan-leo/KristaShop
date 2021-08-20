using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class NewUsersRepository : Repository<NewUser, Guid>, INewUsersRepository {
        private readonly MySqlCompiler _compiler;

        public NewUsersRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<NewUserSqlView>> GetNotApprovedAsync() {
            var compiledSql = _compiler.Compile(_createNotApprovedUsersQuery());
            return await Context.Set<NewUserSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }
        
        public async Task<IEnumerable<NewUserSqlView>> GetGuestsAsync() {
            var compiledSql = _compiler.Compile(_createGuestsUsersQuery());
            return await Context.Set<NewUserSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createNewUsersQuery() {
            return new Query("for1c_new_users AS user")
                .Select("user.id", "user.fullname", "user.city_id", "user.new_city", "user.mall_address",
                    "user.CompanyAddress", "user.phone", "user.email", "user.Login", "user.Password", "user.user_id",
                    "user.manager_id", "user.create_date")
                .SelectRaw("ifnull(`manager`.`name`, '') AS `manager_name`, ifnull(`city`.`name`, '') AS `city_name`")
                .LeftJoin("1c_manager AS manager", "user.manager_id", "manager.id")
                .LeftJoin("1c_city AS city", "user.city_id", "city.id")
                .OrderByDesc("user.create_date").OrderBy("user.fullname");
        }

        private Query _createNotApprovedUsersQuery() {
            return _createNewUsersQuery()
                .WhereNull("user.user_id");
        }

        private Query _createGuestsUsersQuery() {
            return _createNewUsersQuery()
                .Where("user.user_id", "<", "0");
        }
    }
}