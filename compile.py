from subprocess import run
from os import remove as delete
from glob import glob
from shutil import copyfile, rmtree

# Name the file
name = "random"

# Publish the .exe
command = f"dotnet publish -c release --sc /p:PublishSingleFile=true /p:AssemblyName={name} /p:PublishTrimmed=true"

run(command)

bin_name = name + ".exe"
pub_file = "./bin/release/net8.0/win-x64/publish/" + bin_name
out_path = "./sprt/" + bin_name

copyfile(pub_file, out_path)

# Delete all the generated DLLs
for p in glob("./bin/release/net8.0/win-x64/*.dll"):
    delete(p)

# Delete any JSON files
for p in glob("./bin/release/net8.0/win-x64/*.json"):
    delete(p)

# Delete the remaining files
rmtree("./bin/release/net8.0/win-x64/publish")

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