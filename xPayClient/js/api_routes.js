const baseRoute = "http://localhost:5181/api/"
const loginRoute = baseRoute + "Admin/Auth/Login";
const refreshTokenRoute = baseRoute + "Token/refresh";
const sayAutnHelloRoute =  baseRoute + "Admin/Auth/sayAutnHello";
//Accounts
const accountBaseRoute = baseRoute + "Admin/Accounts";
const lockoutUserRoute = accountBaseRoute + "/lockoutUser";
const unlockoutUserRoute = accountBaseRoute + "/unlockoutUser";
const lockoutAllUsersRoute = accountBaseRoute + "/lockoutUsers";
const unlockoutAllUsersRoute = accountBaseRoute + "/unlockoutUsers";
export {baseRoute, loginRoute, refreshTokenRoute, sayAutnHelloRoute, lockoutUserRoute,
lockoutAllUsersRoute, unlockoutAllUsersRoute, unlockoutUserRoute, accountBaseRoute}