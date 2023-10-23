let user = function (jsonUserObject) {
        this.lockoutEnabled = jsonUserObject.LockoutEnabled;
        this.lockoutEndDate = jsonUserObject.LockoutEnd;
        this.username = jsonUserObject.Username;
        this.flag = jsonUserObject.Flag;
        this.savings = jsonUserObject.Savings;
        this.withdrawalLimit = jsonUserObject.WithdrawalLimit;
}
export {user};
