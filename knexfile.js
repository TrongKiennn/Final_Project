require('dotenv').config();

module.exports = {
  development: {
    client: 'mysql2',
    connection: {
      host: `${process.env.MYSQL_HOST}`,
      port: parseInt(process.env.MYSQL_PORT),
      database: `${process.env.MYSQL_DB}`,
      user:     `${process.env.MYSQL_USER}`,
      password: `${process.env.MYSQL_PASSWORD}`
    },
    migrations: {
      tableName: 'knex_migrations'
    }
  },
}