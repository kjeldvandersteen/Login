<?php

$stmt = $conn->prepare("SELECT * FROM users WHERE email = :email");
$stmt->bindparam(":email", $request->email);
$stmt->execute();

$results = $stmt->fetchAll(PDO::FETCH_ASSOC);

if ($results == $request->email) {
    $response->status = "error";
    $response->customMessage = "email bestaat al";
    die (json_encode($response));
}

$stmt = $conn->prepare("SELECT * FROM users WHERE username = :username");
$stmt->bindparam(":username", $request->username);
$stmt->execute();

$results = $stmt->fetchAll(PDO::FETCH_ASSOC);

if ($results == $request->username) {
    $response->status = "error";
    $response->customMessage = "username bestaat al";
    die (json_encode($response));
}

$password = password_hash($request->password, PASSWORD_DEFAULT);
$stmt = $conn->prepare("INSERT INTO users (email, username, password_hash) VALUES (:email, :username, :password_hash)");
$stmt->bindParam(":email", $request->email);
$stmt->bindParam(":username", $request->username);
$stmt->bindParam(":password_hash", $password);
$stmt->execute();


//add token
$stmt = $conn->prepare("SELECT * FROM users WHERE email = :email");
$stmt->bindparam(":email", $request->email);
$stmt->execute();

$result = $stmt->fetch(PDO::FETCH_ASSOC);

if($result != 0){
    $token = generateToken();
    $stmt = $conn->prepare("UPDATE users SET token = :token WHERE user_id = :id");
    $stmt->bindparam(":token", $token);
    $stmt->bindparam(":id", $result['user_id']);
    $stmt->execute();

}

$response->token = $token;
$response->status = "succes";
$response->customMessage = "Het werkt tot nu toe";
die(json_encode($response));
