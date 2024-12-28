exports.seed = async function (knex) {
  // Deletes ALL existing entries
  await knex('ingredients').del();

  // Inserts seed entries
  await knex('ingredients').insert([
    { ingredient_id: 1, name: 'Cà phê', unit: 'gram', stock: 240 },
    { ingredient_id: 2, name: 'Nước', unit: 'mililit', stock: 2500 },
    { ingredient_id: 3, name: 'Sữa tươi', unit: 'mililit', stock: 1320 },
    { ingredient_id: 4, name: 'Milk foam', unit: 'mililit', stock: 120 },
    { ingredient_id: 5, name: 'Sô cô la', unit: 'gram', stock: 60 },
    { ingredient_id: 6, name: 'Đường', unit: 'gram', stock: 190 },
    { ingredient_id: 7, name: 'Trà', unit: 'gram', stock: 43 },
    { ingredient_id: 8, name: 'Bột khoai môn', unit: 'gram', stock: 30 },
    { ingredient_id: 9, name: 'Mat cha', unit: 'gram', stock: 3 },
    { ingredient_id: 10, name: 'Trà ô lông', unit: 'gram', stock: 30 },
    { ingredient_id: 11, name: 'Bột mì', unit: 'gram', stock: 1000 },
    { ingredient_id: 12, name: 'Bơ lạt', unit: 'gram', stock: 450 },
    { ingredient_id: 13, name: 'Men', unit: 'gram', stock: 15 },
    { ingredient_id: 14, name: 'Muối', unit: 'gram', stock: 15 },
    { ingredient_id: 15, name: 'Dâu tây', unit: 'gram', stock: 150 },
    { ingredient_id: 16, name: 'Mật ong', unit: 'gram', stock: 50 },
    { ingredient_id: 17, name: 'Xoài', unit: 'gram', stock: 200 },
    { ingredient_id: 18, name: 'Bơ tươi', unit: 'quả', stock: 1 },
    { ingredient_id: 19, name: 'Hạnh nhân', unit: 'gram', stock: 50 },
    { ingredient_id: 20, name: 'Chuối', unit: 'quả', stock: 1 },
    { ingredient_id: 21, name: 'Cam', unit: 'quả', stock: 4 },
    { ingredient_id: 22, name: 'Carot', unit: 'củ', stock: 3 },
    { ingredient_id: 23, name: 'Táo', unit: 'quả', stock: 3 },
    { ingredient_id: 24, name: 'Nho', unit: 'gram', stock: 200 },
    { ingredient_id: 25, name: 'Trà xanh', unit: 'gram', stock: 10 },
    { ingredient_id: 26, name: 'Hoa cúc khô', unit: 'gram', stock: 5 },
    { ingredient_id: 27, name: 'Lá bạc hà', unit: 'lá', stock: 8 },
    { ingredient_id: 28, name: 'Trà đen', unit: 'gram', stock: 25 },
    { ingredient_id: 29, name: 'Chanh', unit: 'quả', stock: 5 },
    { ingredient_id: 30, name: 'Dừa tươi', unit: 'quả', stock: 1 },
    { ingredient_id: 31, name: 'Chanh leo', unit: 'quả', stock: 2 },
    { ingredient_id: 32, name: 'Việt quốc', unit: 'gram', stock: 150 },
    { ingredient_id: 33, name: 'Mâm xôi', unit: 'gram', stock: 150 },
    { ingredient_id: 34, name: 'Đào', unit: 'quả', stock: 2 },
    { ingredient_id: 35, name: 'Vải thiều', unit: 'gram', stock: 150 },
  ]);
};
