from subprocess import run
from os import remove as delete
from glob import glob
from shutil import rmtree
import re

# Name the file
name = "random"

with open("pitfall.csproj", "r") as f:
    c = f.read()

new_c = c.replace(
    re.findall(
        r"<AssemblyName>.+<\/AssemblyName>",
        c
    )[0],
    f"<AssemblyName>{name}</AssemblyName>"
)

with open("pitfall.csproj", "w") as f:
    f.write(new_c)

# Publish the .exe
command = "dotnet publish -c release --sc /p:PublishSingleFile=true /p:PublishTrimmed=true -o \"./Oppoents/" # sprt/\""

run(command)

# Delete all the generated DLLs
for p in glob("./bin/release/net8.0/win-x64/*.dll"):
    delete(p)

# Delete any JSON files
for p in glob("./bin/release/net8.0/win-x64/*.json"):
    delete(p)

# Delete the remaining files
try:
    rmtree("./bin/release/net8.0/win-x64/publish")
except:
    pass

try:
    delete(f"./sprt/{name}.pdb")
except:
    pass

try:
    delete("./bin/release/net8.0/win-x64/createdump.exe")
except:
    pass

try:
    delete(f"./bin/release/net8.0/win-x64/{name}.exe")
except:
    pass

try:
    delete(f"./bin/release/net8.0/win-x64/{name}.pdb")
except:
    pass