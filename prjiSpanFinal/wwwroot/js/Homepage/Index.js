//const { each } = require("jquery");

const moreitembtn = document.querySelector("#moreitembtn");
const itemcol = document.querySelector("#itemcol");
let itemcount = document.querySelector('#itemCount').value;
let count = 0;
let ruler = 334;
let counter = 1;
async function moreitem() {
    if (count < 5 ) {
        itemcol.style = `height:${343 + (counter * ruler)}px`;
        count += 1;
        counter += 1;
        itemcount -= 6;
        if (count == 5 || itemcount<=6)
            moreitembtn.style.display = "none";
    }
}
moreitembtn.addEventListener("click", async () => {
    await moreitem();
})