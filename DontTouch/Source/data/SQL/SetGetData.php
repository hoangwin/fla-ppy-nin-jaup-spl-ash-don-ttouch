<?php
//khi goi thi goi nhu ben duoi chu y da ta base co it nhat 5 phan tu
//http://gamethuanviet.com/dontstepthewrongtile/SetGetData.php?type=select&username=Hello
//http://gamethuanviet.com/dontstepthewrongtile/SetGetData.php?type=update&username=toanstt&Score=12&Level=31&Played=14&country=14a

$con = mysql_connect("localhost","gamethua_game","30xxxx");
if (!$con)
  {
	
	die('Could not connect: ' . mysql_error());
  }


$typeCommand =  $_GET['type'];

// some code
mysql_select_db("gamethua_thuanviet", $con);

if($typeCommand == "update")
{	

	$UpdateCommand = "INSERT INTO `dontstepthewrongtile` (`UserName`, `Score1`, `Score2`, `Score3`,`Level`, `Played`, `Country`)	" 
	."VALUES ('". $_GET["username"] 
	."'," . $_GET["Score1"] 
	."," . $_GET["Score2"] 
	."," . $_GET["Score3"] 
	. "," .$_GET["Level"]  
	. "," .$_GET["Played"]  
	. ",'" . $_GET["country"] . "'"
	. ") ON DUPLICATE KEY UPDATE "
	. " `Score1` = IF( `Score1`<" . $_GET["Score1"]  . " AND " . $_GET["Score1"]. "> 0," . $_GET["Score1"]. ",`Score1` ),"
	. " `Score2` = IF( `Score2`>" . $_GET["Score2"]  . ", `Score2`," . $_GET["Score2"]. " ),"
	. " `Score3` = IF( `Score3`>" . $_GET["Score3"]  . ", `Score3`," . $_GET["Score3"]. " ),"
	. " `Played` = IF( `Played` >" . $_GET["Played"] . ", `Played`," . $_GET["Played"]. " )";	
	//echo $UpdateCommand;
/*	$file = 'people.txt';	
	$current = file_get_contents($file);	
	$current .=  $UpdateCommand ;	
	file_put_contents($file, $current);
	*/
	
	$result = mysql_query($UpdateCommand);
}else if($typeCommand == "select")
{
	$selectCommand1 = "SELECT * FROM `dontstepthewrongtile` WHERE Score1 > 0 ORDER BY `Score1` ASC LIMIT 0 , 10";
	$selectCommand2 = "SELECT * FROM `dontstepthewrongtile` WHERE Score2 > 0 ORDER BY `Score2` DESC LIMIT 0 , 10";
	$selectCommand3 = "SELECT * FROM `dontstepthewrongtile` WHERE Score3 > 0 ORDER BY `Score3` DESC LIMIT 0 , 10";
	//$Insert_UpdateCommand = "INSERT INTO `dontstepthewrongtile` (`UserName`, `Score`, `Country`, `Unknow`) VALUES (`CaoGia`, 1,'VN','UnKnow') ON DUPLICATE KEY UPDATE `Score` = 1234"
	$result = mysql_query($selectCommand1);
	//echo mysql_num_rows ($result)."|";
	while($row = mysql_fetch_array($result))
	{
		echo $row['UserName'] . "|" . $row['Score1'];
		echo "|";
	}
	
	$result = mysql_query($selectCommand2);
	//echo mysql_num_rows ($result)."|";
	while($row = mysql_fetch_array($result))
	{
		echo $row['UserName'] . "|" . $row['Score2'];
		echo "|";
	}
	
	$result = mysql_query($selectCommand3);
	//echo mysql_num_rows ($result)."|";
	while($row = mysql_fetch_array($result))
	{
		echo $row['UserName'] . "|" . $row['Score3'];
		echo "|";
	}
	
	//user rank
	$selectCommand1 = "SELECT COUNT( * ) AS rank FROM  `dontstepthewrongtile` WHERE  (Score1 > 0) AND (`Score1` < ( SELECT  `Score1` FROM  `dontstepthewrongtile` WHERE  `UserName` ='".$_GET["username"]. "'))";
	$selectCommand2 = "SELECT COUNT( * ) AS rank FROM  `dontstepthewrongtile` WHERE  (Score2 >= 0) AND (`Score2` > ( SELECT  `Score2` FROM  `dontstepthewrongtile` WHERE  `UserName` ='".$_GET["username"]. "'))";
	$selectCommand3 = "SELECT COUNT( * ) AS rank FROM  `dontstepthewrongtile` WHERE  (Score3 >= 0) AND (`Score3` > ( SELECT  `Score3` FROM  `dontstepthewrongtile` WHERE  `UserName` ='".$_GET["username"]. "'))";
	
	//echo $selectCommand;
	$result = mysql_query($selectCommand1);	
	while($row = mysql_fetch_array($result))
	{
		echo ($row['rank'] +1). "|";
	}
	//echo $selectCommand;
	$result = mysql_query($selectCommand2);	
	while($row = mysql_fetch_array($result))
	{
		echo ($row['rank'] +1). "|";
	}
	//echo $selectCommand;
	$result = mysql_query($selectCommand3);	
	while($row = mysql_fetch_array($result))
	{
		echo ($row['rank'] +1). "|";
	}
	//user info
	$username = $_GET["username"];
	
	$selectCommand = "SELECT *   FROM  `dontstepthewrongtile` WHERE  `UserName` =  '".$username."'";
	//echo $selectCommand ;
	$result = mysql_query($selectCommand);
	while($row = mysql_fetch_array($result))
	{
		echo $row['UserName'] . "|" . $row['Score1']. "|" . $row['Score2']. "|" . $row['Score3'];
		echo "|";
	}
}

//end some code
mysql_close($con);
?> 


