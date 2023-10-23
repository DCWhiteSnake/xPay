import { refreshTokenRoute } from "./api_routes.js";

function authenticatedResponseModel(token, refreshToken) {
        this.token = token;
        this.refreshToken = refreshToken;
}
authenticatedResponseModel.prototype.refresh = function () {
        fetch(refreshTokenRoute, { method: "POST", headers: { tokenApiModel: {AccessToken: this.token, RefreshToken: this.refreshToken }} })
                .then((response) => {
                        if (response.status == 200) {
                                return response.json();
                        }
                        else{
                                throw Error(response.statusText);
                        }
                })
                .then((responseJson) => {
                        if (responseJson.Token) {
                                storeAuthValuesInLocalStorage(responseJson);
                                this.token = responseJson.Token;
                                this.refreshToken = responseJson.refreshToken;
                        }
                        else if (responseJson.hasOwnProperty("error")) {
                                let errorMessage = responseJson["error"];
                                let invalidTokenRegex = /^Invalid token/ig;
                                if (invalidTokenRegex.test(errorMessage)) {
                                        alert(errorMessage+ " Please sign in again to revalidate");
                                        setTimeout(() => {
                                                window.location.replace("/login.html");
                                        }, 500);
                                }
                        }
                })
                .catch((error) => {
                        console.log(error);
                        alert(errorMessage+ " Please sign in again to revalidate");
                        setTimeout(() => {
                                window.location.replace("/login.html");
                        }, 500);
                })

}


const storeAuthValuesInLocalStorage = (responseJson) => {
        localStorage.setItem("token", responseJson["token"])
        localStorage.setItem("refreshToken", responseJson["refreshToken"]);
}

export { authenticatedResponseModel, storeAuthValuesInLocalStorage };