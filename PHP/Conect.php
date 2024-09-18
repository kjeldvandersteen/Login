<?php 

$conn = Connect();


$Fruit = new stdClass();
$fruitName = ['fruit_name'];
$fruitColor = ['fruit_color'];
$fruitQuantity = ['fruit_quantity'];


$Fruit->Fruit_name = $fruitName;
$Fruit->Fruit_color = $fruitColor;
$fruit->Fruit_quantity = $fruitQuantity;

//data insert query
$stmt = $conn->prepare("INSERT INTO fruit(fruit_name, fruit_color, fruit_quantity) VALUES (:fruit_name, :fruit_color, :fruit_quantity)");
$stmt->BindParam(":fruit_name", $fruitName);
$stmt->BindParam(":fruit_color", $fruitColor);
$stmt->BindParam(":fruit_quantity", $fruitQuantity);
$stmt->execute();

//Get data from fruit database
$stmt = $conn->prepare("SELECT * FROM fruit");
$stmt->execute();

$result = $stmt->fetchALL(PDO::FETCH_ASSOC);
foreach($result as $row){
    echo "Name: " . $row['fruit_name'] . "  Amount: " . $row['fruit_quantity'] . "<br>";
} 

//data set query
/*$stmt = $conn->prepare( "UPDATE fruit Set fruit_color=:fruit_color WHERE fruit_name = :fruit_name");
$stmt->bindParam(":fruit_color", $fruit_color);
$stmt->bindParam(":fruit_name", $fruit_name);
$stmt->execute();*/

$fruit_color = "Purple";
$fruit_name = "Banana";
//Data Delete query
/*$stmt = $conn->prepare("DELETE FROM fruit WHERE fruit_name = :fruit_name");
$stmt->bindParam(":fruit_name", $fruit_name);
$stmt->execute();*/

//connection with the database
function Connect() {

    //database login variables
    $dbHost = "localhost";
    $dbUsername = "root";
    $dbPasword = "";
    $dbName = "example";

    // try to make connection if it goes wrong echo the problem
    try {
        $pdo = new PDO("mysql:host=$dbHost;dbname=$dbName", $dbUsername, $dbPasword);
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        return $pdo;
    } catch(PDOException $e) {
        echo "Connection doesn't Work:" . $e->getMessage();
    }
}