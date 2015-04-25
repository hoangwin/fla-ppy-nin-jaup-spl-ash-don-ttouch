-------- phpMyAdmin SQL Dump
-- version 3.5.2.2
-- http://www.phpmyadmin.net
--
-- Host: sql106.byethost10.com
-- Generation Time: Apr 21, 2013 at 10:01 PM
-- Server version: 5.5.27-28.0
-- PHP Version: 5.3.5

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

--
-- Database: `b10_11763771_GameLeaderBoard`
--

-- --------------------------------------------------------

--
-- Table structure for table `ArmyDefenderGame`
--

CREATE TABLE IF NOT EXISTS `dontstepthewrongtile` (
  `UserName` varchar(100) NOT NULL,
  `Score1` float(11) NOT NULL,
  `Score2` int(11) NOT NULL,
  `Score3` float(11) NOT NULL,
  `Level` int(11) NOT NULL,
  `Played` int(11) NOT NULL,
  `Country` int(11) NOT NULL,
  PRIMARY KEY (`UserName`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `ArmyDefenderGame`
--

INSERT INTO `dontstepthewrongtile` (`UserName`, `Score1`,`Score2`,`Score3`, `Level`, `Played`, `Country`) VALUES
('Hello', 1,1,1, 1, 1, 0),
('aa', 1, 1,1,1, 1, 1),
('toanstt', 0, 1,1,0, 0, 0);

//


//


DELETE FROM fruitcuttermaster
WHERE score <100