<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.2.5" name="tes_overworld" tilewidth="8" tileheight="8" tilecount="16" columns="4">
 <image source="tes_overworld.png" width="32" height="32"/>
 <tile id="0" type="Grass">
  <properties>
   <property name="Test Property" value="Hello"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" name="left" x="0" y="0" width="2" height="8"/>
   <object id="2" name="right" x="6" y="0" width="2" height="8">
    <properties>
     <property name="TestProp" value="hello"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="0" duration="1000"/>
   <frame tileid="1" duration="1000"/>
   <frame tileid="2" duration="1000"/>
   <frame tileid="3" duration="1000"/>
  </animation>
 </tile>
 <tile id="1" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="2" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="3" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="4" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="4"/>
  </objectgroup>
 </tile>
 <tile id="5" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="4" y="0" width="4" height="8"/>
  </objectgroup>
 </tile>
 <tile id="6" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="0" y="4" width="8" height="4"/>
  </objectgroup>
 </tile>
 <tile id="7" type="Grass">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="4" height="8"/>
  </objectgroup>
 </tile>
 <tile id="8">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="9">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="12" type="Destroyable">
  <properties>
   <property name="Tag" value="Test Tag"/>
  </properties>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <tile id="13" type="Destroyable">
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="8" height="8"/>
  </objectgroup>
 </tile>
 <wangsets>
  <wangset name="New Wang Set" tile="-1"/>
 </wangsets>
</tileset>
