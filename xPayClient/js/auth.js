import { loginRoute } from "./api_routes.js";
import { authenticatedResponseModel, storeAuthValuesInLocalStorage } from "./auth_helper.js";
import { displayLoadingSymbol, delay } from "./helpers.js";

// if (window.location.href == "/login.html") {

        document.addEventListener("DOMContentLoaded", function () {
                const form = document.getElementById('adminLoginForm'); // Or use document.querySelector() with a CSS selector
                form.addEventListener('submit', function (event) {
                        event.preventDefault();
                        const username = form.elements.username.value;
                        const password = form.elements.password.value;
                        if (username != "" && password != "") {
                                fetch(loginRoute, { method: "POST", body: new FormData(form) })
                                        .then(response => {
                                                return response.json();
                                        })
                                        .then(async responseJson => {
                                                if (responseJson.hasOwnProperty("error")) {
                                                        console.log(responseJson["error"]);
                                                }
                                                else if (responseJson.hasOwnProperty("token")) {
                                                        storeAuthValuesInLocalStorage(responseJson);
                                                        displayLoadingSymbol();
                                                        await delay(5000);
                                                        window.location.replace("/");
                                                }
                                        })
                                        .catch((error) => {
                                                console.log(error);
                                        })
                        }
                        else {
                                alert("Please fill in the values");
                        }
                });


        });


const logout = () => {
        localStorage.clear();
        window.location.replace("/login.html");
}

export { logout };