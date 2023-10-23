

const displayLoadingSymbol = () => {
        let loadingSymbolContainer = document.createElement("div");
        loadingSymbolContainer.classList.add("loading-symbol-container");
        let loadingSymbol = document.createElement("img");
        loadingSymbol.classList.add("loading-symbol");
        loadingSymbol.src = "../img/loading_sym1.gif";
        loadingSymbolContainer.appendChild(loadingSymbol);
        document.querySelector("body").appendChild(loadingSymbolContainer);
}
const delay = ms => new Promise(res => setTimeout(res, ms));

const displaySuccessfulSymbol = (targetNode) => {
       let tick =  document.querySelector("#" + targetNode.id + "SuccessTick")
       tick.style.display = "inline"
       setTimeout(() => {tick.style.display = "none"}, 1500);
}
export {displayLoadingSymbol, delay, displaySuccessfulSymbol};

