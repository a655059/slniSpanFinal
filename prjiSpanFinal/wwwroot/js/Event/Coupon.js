﻿const couponbtn = document.getElementById("coupon-btn");
const couponbtntext = document.getElementById("coupon-btn-text");
console.log(couponbtn);
console.log(couponbtntext);
let i = 0;

couponbtn.addEventListener("click", () => {
    if (i == 0) {
        couponbtntext.setAttribute(`style`,"border:2px solid #FF5809;background-color:white;color:#FF5809;")
        couponbtntext.textContent = "去逛逛";
    }
    if (i >= 1) {
        couponbtn.setAttribute("href", "/Category/Index");
    }
    console.log(i);
    i+=1;
})