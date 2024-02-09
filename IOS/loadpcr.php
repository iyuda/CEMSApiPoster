<?php

$servername = "localhost";

$username = "root";

$password = "secret";

$dbname = "ios";

$id = $_GET['id'];
$agency = $_GET['agency'];
$user = $_GET['user'];

$conn = new mysqli($servername, $username, $password, $dbname);

$payload =file_get_contents('http://localhost:53184/api/CEMS/GetPcrFromEMS?pcr_id='.$id .'&agency_id='.$agency.'&user='.$user);
echo $payload;

$sqlUpdate = "update newcems.dispatching set dispatch_switch=0 where pcr_id = '" . $id . "'";
$return = $conn->query($sqlUpdate);

$sqlSelect = "select id, CAST(data AS CHAR) as data from json_pcr where id = '" . $id . "' and is_upload=0";



$return = $conn->query($sqlSelect);
$err = mysqli_error($conn);     //$conn->error

if ($return->num_rows > 0) {
	while ($row = $return->fetch_assoc()) {
		
	}
	
}
 
$conn->close();
?>
