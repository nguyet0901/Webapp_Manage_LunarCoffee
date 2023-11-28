function cart_addToCart(item) {
    if (item && Object.keys(item).length > 0) {
        let cart = JSON.parse(author_get('cart') || "{}");
        if (cart[item.ID]) {
            cart[item.ID].Quantity += 1;
        }
        else {
            cart[item.ID] = {
                ID : item.ID,
                TypeID: item.TypeID,
                Quantity: 1,
                Price: item.Price,
                DiscountAmount: item?.DiscountAmount ?? 0,
                DiscountPercent: item?.DiscountPercent ?? 0
            }
        }

        author_set('cart', JSON.stringify(cart)); 
        notiSuccess('Thêm vào giỏ hàng thành công');
    }
    $('#master_totalCart').html(cart_getTotalPro());
}

function cart_editCart(item) {
    if (item && Object.keys(item).length > 0) {
        let cart = JSON.parse(author_get('cart') || "{}");
        if (cart[item.ID]) {
            cart[item.ID].Quantity = item.Quantity;
        }

        author_set('cart', JSON.stringify(cart));
    }
    $('#master_totalCart').html(cart_getTotalPro());
}


function cart_deleteCart(id) {
    let cart = JSON.parse(author_get('cart') || "{}");
    if (cart[id]) {
        delete cart[id];
    }

    author_set('cart', JSON.stringify(cart));
    notiSuccess('Xóa thành công');
    $('#master_totalCart').html(cart_getTotalPro());
}

function cart_getTotalPro() {
    let cart = JSON.parse(author_get('cart') || "{}");
    return Object.values(cart)?.length ?? 0;
}

function cart_getCartList() {
    return JSON.parse(author_get('cart') || "{}");
}