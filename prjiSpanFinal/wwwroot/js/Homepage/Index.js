const moreitembtn = document.querySelector("#moreitembtn");
const itemcol = document.querySelector("#itemcol");
let count = 0;
let ruler = 6;
let counter = 1;
async function moreitem() {
    if (count < 5) {
        let x = "";
        for (let i = 0; i < 6; i++) {
            x += `@foreach (var item in Model.Skip(${6 + (ruler * (counter-1))}).Take(${ruler * counter})){<div class="sellsitem"><a class="linknoline" href="/Item/Index/2113"> @if (item.Pic != null){var base64Image = Convert.ToBase64String(item.Pic);var source = String.Format("data:image/png;base64,{0}", base64Image);<img href="/Item/Index/2113" src="@source" class="cardImg" alt="..." />} else {<img href="/Item/Index/2113" src="#" class="cardImg" alt="此商品沒有圖片" />}</a><div class="px-1 d-flex flex-column justify-content-between"><div class="cardTitle">@item.Product.ProductName</div>@{ if (item.Price.Count == 1){<div class="cardPrice">$@decimal.Round(item.Price[0])</div>} else{<div class="cardPrice">$@decimal.Round(item.Price[0]) - $@decimal.Round(item.Price[1])</div>}}<div class="cardInfo d-flex justify-content-between align-items-end"><div class="d-flex justify-content-start flex-column mt-1"><div class="d-flex justify-content-start"><div class="starImg"><img src="/img/YellowStar.png" alt="" class="me-1 d-block" /></div><div class="text-black">4.5</div></div><div class="text-black-50">銷售<span>100</span></div></div><div><button type="button" class="btn btn-outline-warning buybtn">加入購物車</button></div></div></div></div>}`+" ";
        }
        itemcol.innerHTML += x;
        count += 1;
        counter += 1;
        if (count == 5)
            moreitembtn.style.display = "none";
    }
}
moreitembtn.addEventListener("click", async () => {
    await moreitem();
})