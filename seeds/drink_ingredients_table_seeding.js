exports.seed = async function (knex) {
  await knex('drink_ingredients').del();

  await knex('drink_ingredients').insert([
    // Espresso
    { drink_id: 1, ingredient_id: 1, quantity: 10 },
    { drink_id: 1, ingredient_id: 2, quantity: 30 },

    // Americano
    { drink_id: 2, ingredient_id: 1, quantity: 10 },
    { drink_id: 2, ingredient_id: 2, quantity: 90 },

    // Latte
    { drink_id: 3, ingredient_id: 1, quantity: 15 },
    { drink_id: 3, ingredient_id: 3, quantity: 200 },
    { drink_id: 3, ingredient_id: 4, quantity: 30 },

    // Cappuccino
    { drink_id: 4, ingredient_id: 1, quantity: 10 },
    { drink_id: 4, ingredient_id: 3, quantity: 30 },
    { drink_id: 4, ingredient_id: 4, quantity: 30 },

    // Mocha
    { drink_id: 5, ingredient_id: 1, quantity: 10 },
    { drink_id: 5, ingredient_id: 5, quantity: 30 },
    { drink_id: 5, ingredient_id: 3, quantity: 90 },

    // Milk Tea Original
    { drink_id: 6, ingredient_id: 6, quantity: 8 },
    { drink_id: 6, ingredient_id: 3, quantity: 100 },
    { drink_id: 6, ingredient_id: 7, quantity: 20 },

    // Taro Milk Tea
    { drink_id: 7, ingredient_id: 3, quantity: 100 },
    { drink_id: 7, ingredient_id: 7, quantity: 15 },
    { drink_id: 7, ingredient_id: 8, quantity: 30 },
    { drink_id: 7, ingredient_id: 6, quantity: 8 },

    // Matcha Milk Tea
    { drink_id: 8, ingredient_id: 9, quantity: 3 },
    { drink_id: 8, ingredient_id: 3, quantity: 200 },
    { drink_id: 8, ingredient_id: 7, quantity: 20 },
    { drink_id: 8, ingredient_id: 2, quantity: 50 },

    // Oolong Milk Tea
    { drink_id: 9, ingredient_id: 10, quantity: 7 },
    { drink_id: 9, ingredient_id: 3, quantity: 150 },
    { drink_id: 9, ingredient_id: 7, quantity: 20 },

  ]);
};
