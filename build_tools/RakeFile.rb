require 'fileutils'
TARGETDIR = 'C:/inetpub/wwwroot'
SVNPATH = '"C:/Program Files/SlikSvn/bin/svn.exe"'
MSBUILDPATH = 'C:/WINDOWS/Microsoft.NET/Framework/v4.0.30319/msbuild.exe '
APPPOOLNAME = 'DefaultAppPool'
SVNWEBDIRECTORY = 'C:/3dr'
SVNWEBPROJECTDIRECTORY = File.join(SVNWEBDIRECTORY,'VwarWeb')
CONFIGFILE = File.join(SVNWEBDIRECTORY,'config','web.config.stage')
SVNTOOLSDIRECTORY = 'C:/3dtools'
TOOLSBUILDPATH = File.join(SVNTOOLSDIRECTORY,"bin")
COMMUNITYCONFIGFILE = File.join(SVNWEBDIRECTORY,'config','community.config.stage')
def svn_update(path)
        sh SVNPATH + ' cleanup ' + path
        sh SVNPATH + ' revert ' + path + ' --depth=infinity'
		sh SVNPATH + ' update ' + path
end
def msbuild_run(path)
        sh MSBUILDPATH + path +' /t:Clean'
        sh MSBUILDPATH + path +' /p:Platform="Any CPU" '
end
task :update do
        svn_update(SVNWEBDIRECTORY)
        svn_update(SVNTOOLSDIRECTORY)
end
task :build do
#        msbuild_run File.join(SVNTOOLSDIRECTORY, 'ConverterWrapper.sln')        
       # sh SVNPATH +  ' export --force -v ' + TOOLSBUILDPATH + ' ' + File.join(SVNWEBPROJECTDIRECTORY,"bin/")
        msbuild_run File.join(SVNWEBDIRECTORY, 'VwarSolution.sln')
end
task :deploy do

	begin
		sh 'appcmd stop apppool /apppool.name:' + APPPOOLNAME
	rescue
		puts APPPOOLNAME + ' is already stopped.'
	end
	
        if File.exists?(TARGETDIR)
				rm_rf TARGETDIR
        end        
        if File.exists?(File.join('C:/inetpub/','community'))
                rm_rf File.join('C:/inetpub/','community')
        end        
        sh SVNPATH +  ' export --force ' + SVNWEBPROJECTDIRECTORY + ' ' + TARGETDIR
	cp CONFIGFILE, File.join(TARGETDIR, 'web.config')
    cp File.join(SVNWEBPROJECTDIRECTORY ,'bin/VwarDal.dll'), File.join(TARGETDIR,'bin/VwarDal.dll')
	sh SVNPATH +  ' export ' + File.join(SVNWEBDIRECTORY,'DMG_Forums_3-2') + ' ' + File.join('C:/inetpub/','community')
	cp COMMUNITYCONFIGFILE, File.join(File.join('C:/inetpub/','community'), 'web.config')
	
	cp FileList.new(File.join(TOOLSBUILDPATH, 'converterDLL.dll'), File.join(TOOLSBUILDPATH, 'ConverterWrapper.dll')), File.join(TARGETDIR, 'bin/')
	
	#sh 'Icacls ' + TARGETDIR + '/App_Data /t /grant Users:f'
	
	sh 'appcmd start apppool /apppool.name:' + APPPOOLNAME
end
task :default => [:update, :build, :deploy] do
        puts 'Done'
end