<?php
$email = $request->email;
$username = $request->username;

$stmt = $conn->prepare("SELECT * FROM users WHERE email = :email");
$stmt->bindparam(":email", $email);
$stmt->execute();

$results = $stmt->fetchAll(PDO::FETCH_ASSOC);

if ($results == $request->email) {
    $response->status = "error";
    $response->customMessage = "email bestaat al";
    die (json_encode($response));
}

$stmt = $conn->prepare("SELECT * FROM users WHERE username = :username");
$stmt->bindparam(":username", $username);
$stmt->execute();

$results = $stmt->fetchAll(PDO::FETCH_ASSOC);

if ($results == $request->username) {
    $response->status = "error";
    $response->customMessage = "username bestaat al";
    die (json_encode($response));
}

$password = password_hash($request->password, PASSWORD_DEFAULT);
$stmt = $conn->prepare("INSERT INTO users (email, username, password_hash) VALUES (:email, :username, :password_hash)");
$stmt->bindParam(":email", $email);
$stmt->bindParam(":username", $username);
$stmt->bindParam(":password_hash", $password);
$stmt->execute();


$response->status = "succes";
$response->customMessage = "Het werkt tot nu toe";
die(json_encode($response));
