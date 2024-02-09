<?php

$agency = $_GET['agency'];
$user = $_GET['user'];

$servername = "localhost";

$username = "root";

$password = "secret";

$dbname = "newcems";

file_get_contents('http://localhost/cemslocal/yiicems/IOS/loadpcr.php?id='.$row["pcr_id"].'&agency='.$agency.'&user='.$user);
$payload =file_get_contents('http://localhost:53184/api/CEMS/GetDispatching?agency_id='.$agency.'&user='.$user);
echo $payload;


?>