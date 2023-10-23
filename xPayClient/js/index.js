import { lockoutUserRoute, sayAutnHelloRoute, lockoutAllUsersRoute, baseRoute, accountBaseRoute, unlockoutUserRoute, unlockoutAllUsersRoute } from "./api_routes.js";
import { logout } from "./auth.js"
import { authenticatedResponseModel, storeAuthValuesInLocalStorage } from "./auth_helper.js";
import { displaySuccessfulSymbol } from "./helpers.js";
import { user } from "./user.js";
let credCheckCount = 0;
let token = localStorage.getItem("token");
const checkLogin = () => {

        let refreshToken = localStorage.getItem("refreshToken");
        if (token != "" || token != null) {
                fetch(sayAutnHelloRoute, {
                        method: "GET",
                        headers: {
                                'Authorization': `Bearer ${token}`
                        }
                })
                        .then((response) => {
                                credCheckCount++;
                                if (response.status == 200) {
                                        return response.json();
                                }
                                // If we've run this check at least once.
                                else if (response.status == 401 && credCheckCount != 1) {
                                        let authenticatedResponse = new authenticatedResponseModel(token, refreshToken);
                                        authenticatedResponse.refresh();
                                        return {
                                                token: authenticatedResponse.token,
                                                refreshToken: authenticatedResponse.refreshToken
                                        };
                                }
                                else {
                                        throw Error("Invalid authentication");
                                }
                        })
                        .then((responseJson) => {
                                // if (responseJson.hasOwnProperty("greeting")) {
                                //         console.log(responseJson.greeting);
                                // }
                                if (responseJson.hasOwnProperty("token")) {
                                        storeAuthValuesInLocalStorage(responseJson);
                                }
                        })
                        .catch((error) => {

                                window.location.href = "/login.html";

                        })
        }
}
setInterval(checkLogin, 100000);

document.addEventListener("DOMContentLoaded", () => {

        let logoutBtn = document.querySelector("#logoutBtn");
        logoutBtn.addEventListener('click', logout);
        let featureBtns = document.querySelectorAll(".nav-link");
        featureBtns.forEach((fBtn) => {
                fBtn.addEventListener("click", () => {
                        hideOtherFeatureBoxes(fBtn.name);
                        document.querySelector(`div[name=${fBtn.name}]`).classList.remove("hidden");
                })
        })

        let lockOutSingleUserForm = document.querySelector("#lockSingleForm");
        let token = localStorage.getItem("token");
        lockOutSingleUserForm.addEventListener('submit', (e) => {
                e.preventDefault();
                fetch(lockoutUserRoute, {
                        method: "POST", headers: {
                                'Authorization': `Bearer ${token}`
                        }, body: new FormData(e.target)
                })
                        .then((response) => {
                                if (response.status == 200) {
                                        displaySuccessfulSymbol(e.target)
                                }
                        })

        })
        let unlockOutSingleUserForm = document.querySelector("#unlockSingleForm");
        unlockOutSingleUserForm.addEventListener('submit', (e) => {
                e.preventDefault();
                fetch(unlockoutUserRoute, {
                        method: "POST", headers: {
                                'Authorization': `Bearer ${token}`
                        }, body: new FormData(e.target)
                })
                        .then((response) => {
                                if (response.status == 200) {
                                        displaySuccessfulSymbol(e.target)
                                }
                        })
        });
        let lockOutAllUsersForm = document.querySelector("#lockAllForm");
        lockOutAllUsersForm.addEventListener('submit', (e) => {
                e.preventDefault();
                fetch(lockoutAllUsersRoute, {
                        method: "POST", headers: {
                                'Authorization': `Bearer ${token}`
                        }, body: new FormData(e.target)
                })
                        .then((response) => {
                                if (response.status == 200) {
                                        displaySuccessfulSymbol(e.target)
                                }
                        })

        })
        let unlockOutAllUsersForm = document.querySelector("#unlockAllForm");
        unlockOutAllUsersForm.addEventListener('submit', (e) => {
                e.preventDefault();
                fetch(unlockoutAllUsersRoute, {
                        method: "POST", headers: {
                                'Authorization': `Bearer ${token}`
                        }
                })
                        .then((response) => {
                                if (response.status == 200) {
                                        displaySuccessfulSymbol(e.target)
                                }
                        })

        })

})

let filterUsersRoutes = {
        "Default": accountBaseRoute + "/users",
        "Locked": accountBaseRoute + "/locked",
        "Flagged": accountBaseRoute + "/flagged",
}
let filterSearchForm = document.querySelector("#listAccountsByFilterForm");
filterSearchForm.addEventListener("submit", (e) => {
        e.preventDefault();
        let selectElement = document.querySelector("#filterSelector");
        var selectedValue = selectElement.value;
        var queryRoute = filterUsersRoutes[selectedValue];

        fetch(queryRoute, {
                method: "GET", headers: {
                        'Authorization': `Bearer ${token}`
                }
        })
                .then((response) => {
                        if (response.ok) {
                                return response.json();
                        }
                })
                .then((listOfUsers) => {
                        let uTableContainer = document.querySelector("#listAccountsByFilterOutput");
                        let tBody = document.querySelector("#listAccountsBody")
                        uTableContainer.style.display = "block";
                        while (tBody.firstChild) {
                                tBody.removeChild(tBody.firstChild);
                        }
                        for (let userJson in listOfUsers) {

                                tBody.appendChild(createTableRow(listOfUsers[userJson]));
                        }
                })
})
let generateReportBtn = document.querySelector("#monthlyReportButton");
generateReportBtn.addEventListener("click", (e) => {
        e.preventDefault();

        fetch("https://localhost:7154/api/Admin/Reports", {
                method: "GET", headers: {
                        'Authorization': `Bearer ${token}`
                }
        })
                .then(response => response.blob())
                .then(blob => {
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url;
                        a.download = 'OctoberFinancialReport.pdf'; // Specify the desired file name
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
                })
                .catch(error => {
                        console.error('Error downloading the file:', error);
                });
});

const hideOtherFeatureBoxes = (name) => {
        document.querySelectorAll(".featureBox").forEach((fBox) => {
                if (fBox.name != name) {
                        fBox.classList.add("hidden");
                }
        });
}

const createTableRow = (jsonUserObject) => {
        let tr = document.createElement("tr")
        let td1 = document.createElement("td")
        let td2 = document.createElement("td")
        let td3 = document.createElement("td")
        let td4 = document.createElement("td")
        let td5 = document.createElement("td")
        let td6 = document.createElement("td")
        tr.appendChild(td1);
        tr.appendChild(td2);
        tr.appendChild(td3);
        tr.appendChild(td4);
        tr.appendChild(td5);
        tr.appendChild(td6);
        td1.textContent = jsonUserObject.username;
        td2.textContent = jsonUserObject.flag;
        td3.textContent = jsonUserObject.lockoutEnabled;
        td4.textContent = jsonUserObject.lockoutEnd;
        td5.textContent = jsonUserObject.savings;
        td6.textContent = jsonUserObject.withdrawalLimit;
        return tr;

}