/*
Navicat SQLite Data Transfer

Source Server         : PokerDB
Source Server Version : 30714
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30714
File Encoding         : 65001

Date: 2014-09-04 08:40:40
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for FaceBook
-- ----------------------------
DROP TABLE IF EXISTS "main"."FaceBook";
CREATE TABLE "FaceBook" (
"ID"  INTEGER NOT NULL,
"Login"  TEXT,
"Pass"  TEXT,
"FBID"  TEXT,
"MBLoginText"  TEXT,
"MBCookie"  BLOB,
"WebCookie"  BLOB,
PRIMARY KEY ("ID")
);

-- ----------------------------
-- Table structure for Package
-- ----------------------------
DROP TABLE IF EXISTS "main"."Package";
CREATE TABLE "Package" (
"ID"  INTEGER NOT NULL,
"Pack"  INTEGER,
PRIMARY KEY ("ID")
);

-- ----------------------------
-- Table structure for Poker
-- ----------------------------
DROP TABLE IF EXISTS "main"."Poker";
CREATE TABLE "Poker" (
"ID"  INTEGER NOT NULL,
"FaceBookID"  INTEGER,
"PKID"  TEXT,
"PackageID"  INTEGER,
"MBAccessToken"  TEXT,
"WebAccessToken"  TEXT,
"MBLoginText"  TEXT,
"WebLoginText"  TEXT,
"WebCookie"  BLOB,
"X_TUNNEL_VERIFY"  TEXT,
PRIMARY KEY ("ID" ASC),
CONSTRAINT "Poker_FaceBook" FOREIGN KEY ("FaceBookID") REFERENCES "FaceBook" ("ID"),
CONSTRAINT "Poker_Package" FOREIGN KEY ("PackageID") REFERENCES "Package" ("ID")
);

DROP TABLE IF EXISTS "main"."Tiki";
CREATE TABLE "Tiki" (
"ID"  INTEGER NOT NULL,
"Login"  TEXT,
"Pass"  TEXT,
"Cookie"  BLOB,
PRIMARY KEY ("ID")
);
