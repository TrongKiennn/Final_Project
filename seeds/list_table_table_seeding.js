exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('tableManager').del();

  // Inserts data from id 1 to 30
  await knex('tableManager').insert(
    Array.from({ length: 30 }, (_, i) => ({ id: i + 1 }))
  );
};
