﻿const moreitembtn = document.querySelector("#moreitembtn");
const itemcol = document.querySelector("#itemcol");
let count = 0;
async function moreitem() {
    if (count < 5) {
        let x = "";
        for (let i = 0; i < 6; i++) {
            x += `<div class="sellsitem"><a href="/Item/Index/2113"><img href="/Item/Index/2113" src="/img/蘋果圖.jpg" class="cardImg" alt="..." /></a><div class="px-1 d-flex flex-column justify-content-between"><div class="cardTitle">好大一根香蕉</div><div class="cardPrice">$999</div><div class="cardInfo d-flex justify-content-between align-items-end"><div class="d-flex justify-content-start flex-column mt-1"><div class="d-flex justify-content-start"><div class="starImg"><img src="/img/YellowStar.png" alt="" class="me-1 w-100 d-block" /></div><div class="text-black">4.5</div></div><div class="text-black-50">銷售<span>100</span></div></div><div><button type="button" class="btn btn-outline-warning buybtn">加入購物車</button></div></div></div></div>`+" ";
        }
        itemcol.innerHTML += x;
        count += 1;
        if (count == 5)
            moreitembtn.style.display = "none";
    }
}
moreitembtn.addEventListener("click", async () => {
    await moreitem();
})