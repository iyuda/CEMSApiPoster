<?php
$items = array();

$list = $_GET['list'];
$file='c:\\temp\\list.txt';
file_put_contents($file, $list );
$items["list"] = $list;

array_push($items, array( "id"=>0, "value"=>"list item one" ));
array_push($items, array( "id"=>1, "value"=>"list item two" ));
array_push($items, array( "id"=>2, "value"=>"list item three" ));
array_push($items, array( "id"=>3, "value"=>"list item four" ));
array_push($items, array( "id"=>4, "value"=>"list item five" ));
array_push($items, array( "id"=>5, "value"=>"list item six" ));
array_push($items, array( "id"=>6, "value"=>"list item seven" ));


echo json_encode($items);
?>