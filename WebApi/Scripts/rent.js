var app = angular.module("myApp", []);
app.controller("myCtrl", ($scope, $http, $timeout) => {
    $http.get("api/Books").then((response) => {
        $scope.books = response.data;        
    });
    //----------------------------------- Get Session-------------------------------------------------    
    $http.get("api/Students/PassStudentSession").then((response) => {
        $scope.sessionsStudent = response.data;
    });
    
    $scope.getBook = (id) => {
        $http.get("api/Books/" + id).then((response) => {
            $scope.book = response.data;
            $scope.rentNow = "OK";
        });
    };
    $scope.createOrder = () => {        
        if ($scope.rentNow != ""){            
            $http.post("api/RentalDetails", $scope.rent).then((response) => {
                try {
                    $scope.rentaldetail.push(response.data);
                    alert("Rent Book Successfully!");
                } catch (e) {
                    alert("Rent Book unsuccessfully!");
                }
            });
        };
    };
    $("#dateRent").change(() => {
        let value = $("#dateRent").val();
        let cd = new Date(value);
        var datepay = new Date(cd.setDate(cd.getDate() + 15));
        $scope.rent.DateRent = new Date(value);
        $scope.rent.DatePay = datepay;
        $("#datePay").val(datepay);
    });

    $("#check").change(() => {
        let value = $("#check").val();
        let cd = new Date(value);
        var totalday = cd.setDate(cd.getDate() - $scope.rentaldetail.DatePay).val();
        $("#feepay").val(totalday * 10);
    });
});