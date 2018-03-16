var app = angular.module("myApp", []);
app.controller("myCtrl", ($scope, $http, $timeout) => {
    $scope.addBook = "Add";
    // ------------------------BOOK---------------------------------    
    // get book
    $http.get("api/Books").then((response) => {
        $scope.books = response.data;
        $timeout(() => {
            $("#bookTable").DataTable({
                lengthMenu: [9, 6]
            });
        });
    });
    // get book by ID
    $scope.getBook = (id) => {
        $http.get("api/Books/" + id).then((response) => {
            $scope.book = response.data;
            $scope.addBook = "Update";
            //addReadonlyBook();
            //unReadonlyBook();
        });
    };
    // refresh table
    $scope.refreshBookTable = () => {
        $http.get("api/Books").then((response) => {
            $scope.books = response.data;
            $("#bookTable").DataTable().destroy();
            $timeout(() => {
                $("#bookTable").DataTable({
                    lengthMenu: [9, 6]
                });
            });
        });
    };

    // Get Author
    $http.get("api/Books/GetAuthor").then((response) => {
        $scope.authors = response.data;        
    });
    // Get Category
    $http.get("api/Books/GetCategory").then((response) => {
        $scope.categorys = response.data;
    });
    // add
    $scope.addBookClick = () => {
        $scope.book = new Object();
        $scope.book.CallNumber = 0;
        $scope.addBook = "Add";       
        //unReadonlyBook();
    };
    // update
    $scope.updateBookClick = function () {
        if ($scope.book.CallNumber == 0) {
            $scope.book.AuthorId = $scope.selectedAuthorId.AuthorId;
            $scope.book.CategoryId = $scope.selectedCategoryId.CategoryId;
            $http.post("api/Books", $scope.book).then((response) => {
                $scope.books.push(response.data);
                $scope.addBookClick();
                $scope.refreshBookTable();
                alert("Create book successfully!")

            });
        }
        else {
            $http.put("api/Books/" + $scope.book.CallNumber, $scope.book).then((response) => {
                var index = $scope.books.findIndex(a => a.CallNumber == $scope.book.CallNumber);
                $scope.books[index] = $scope.book;
                $scope.addBookClick();
                $scope.refreshBookTable();
                alert("Edit book successfully!")
            });
        };
    };
    $scope.bookDelete = (id) => {
        var msg = confirm("Are you sure?");
        if (msg) {
            $http.delete("api/Books/" + id).then((response) => {
                var index = $scope.books.findIndex(b => b.CallNumber == id);
                $scope.books.splice(index, 1);
                $scope.addBookClick();
                $scope.refreshBookTable();
                alert("Delete book successfully!")
            });
        }
    };
    
    $scope.book = new Object();
    $scope.book.CallNumber = 0;

    // -----------------------------STUDENT-----------------------------
    $scope.addStudent = "Add";
    $http.get("api/Students").then(function (response) {
        $scope.students = response.data;
        $timeout(function () {
            $("#stuTable").DataTable({
                lengthMenu: [9, 6]
            });
        });
    });

    $http.get("api/Books/GetMajor").then((response) => {
        $scope.majors = response.data;
    });
    // get Student by ID
    $scope.getStudent = (id) => {
        $http.get("api/Students/" + id).then((response) => {
            $scope.student = response.data;
            $scope.addStudent = "Update";
        });
    };
    // refresh Student table
    $scope.refreshStudentTable = () => {
        $http.get("api/Students").then((response) => {
            $scope.students = response.data;
            $("#stuTable").DataTable().destroy();
            $timeout(() => {
                $("#stuTable").DataTable({
                    lengthMenu: [9, 6]
                });
            });
        });
    };
    // add Student
    $scope.addStudentClick = () => {
        $scope.student = new Object();
        $scope.student.StudentId = 0;
        $scope.addStudent = "Add";
        $scope.refreshStudentTable();
    };
    // update Student
    $scope.updateStudentClick = function () {
        $scope.student.MajorId = $scope.selectedMajor.MajorId;
        if ($scope.student.StudentId == 0) {
            $http.post("api/Students", $scope.student).then((response) => {
                $scope.students.push(response.data);
                $scope.addStudentClick();
                alert("Create Student successfully!")
            });
        }
        else {
            $http.put("api/Students/" + $scope.student.StudentId, $scope.student).then((response) => {                
                var index = $scope.students.findIndex(a => a.StudentId == $scope.student.StudentId);
                $scope.student[index] = $scope.student;
                $scope.addStudentClick();
                alert("Edit Student successfully!")
            });
        };
    };
    $scope.studentDelete = (id) => {
        $http.delete("api/Students/" + id).then((response) => {
            var index = $scope.students.findIndex(b => b.StudentId == id);
            $scope.students.splice(index, 1);
            $scope.addStudentClick();
            alert("Delete book successfully!")
        });
    };
    $scope.student = new Object();
    $scope.student.StudentId = 0;

    // -----------------------------Rental Details-----------------------------
    $scope.addRent = "Add";
    $http.get("api/RentalDetails").then(function (response) {
        $scope.rentaldetails = response.data;
        $timeout(function () {
            $("#rentTable").DataTable({
                lengthMenu: [9, 6]
            });
        });
    });
    // get Rental Details by ID
    $scope.getRentalDetails = (id, stuId) => {
        $http.get("api/RentalDetails/" + id + "?stuId=" + stuId).then((response) => {
            $scope.rentaldetail = response.data;
            $scope.rentaldetail.DateRent = new Date(
                response.data.DateRent);
            $scope.rentaldetail.DatePay = new Date(
                response.data.DatePay);
            $scope.addRent = "Update";
        });
    };
    $scope.refreshRentTable = () => {
        $http.get("api/RentalDetails").then((response) => {
            $scope.rentaldetails = response.data;
            $("#rentTable").DataTable().destroy();
            $timeout(() => {
                $("#rentTable").DataTable({
                    lengthMenu: [9, 6]
                });
            });
        });
    };
    $scope.addRentClick = () => {
        $scope.rentaldetail = new Object();
        $scope.rentaldetail.CallNumber = 0;
        $scope.rentaldetail.StudentId = 0;
        $scope.addRent = "Add";
        $scope.refreshRentTable();
    };



    // -----------------------------------RENT BOOK--------------------------------------------------
    $scope.updateRentClick = function () {
        if ($scope.rentaldetail.CallNumber == 0 && $scope.rentaldetail.StudentId == 0) {
            $http.post("api/RentalDetails", $scope.rentaldetail).then((response) => {
                $scope.rentaldetails.push(response.data);
                $scope.updateRentClick();
            });
        }
        else {
            $http.put("api/RentalDetails/" + $scope.rentaldetail.CallNumber, $scope.rentaldetail.StudentId, $scope.rentaldetail).then((response) => {
                var index = $scope.rentaldetails.findIndex(a => a.CallNumber == $scope.rentaldetail.CallNumber && a.StudentId == $scope.rentaldetail.StudentId);
                $scope.rentaldetail[index] = $scope.rentaldetail;
                $scope.updateRentClick();
                alert("Update successfully")
            });
        };
    };
    // -----------Get Session
    $http.get("api/Books/PassSession").then((response) => {
        $scope.sessions = response.data;
    });
    
});