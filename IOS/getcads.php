<?php

$bus = $_GET['bus'];
$agency = $_GET['agency'];

$servername = "localhost";

$username = "root";

$password = "";

$dbname = "newcems";

$sqlSelect = "select info from dispatching d inner join agency a on d.agency_id=a.id inner join users u on d.user_id = u.id where dispatch_switch=1 and dispatch_enum=3 and d.agency_id = '".
	      $agency." and bus_number = '". $bus."'";
$cads = array();

$conn = new mysqli($servername, $username, $password, $dbname);

$return = $conn->query($sqlSelect);
$err = mysqli_error($conn);     //$conn->error
if ($return->num_rows > 0) {
	while ($row = $return->fetch_assoc()) {
				echo $row["info"];
		file_get_contents('http://localhost/cemslocal/yiicems/IOS/loadcad.php?id='.$row["info"] );
	}
}
 
?>

