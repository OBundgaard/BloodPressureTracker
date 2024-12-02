pipeline {
    agent any // Use the default Jenkins node
    environment {
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
    }
    stages {
        stage('Checkout') {
            steps {
                echo 'Checking out source code...'
                checkout scm
            }
        }
        stage('Restore') {
            steps {
                echo 'Restoring NuGet packages...'
                sh 'dotnet restore'
            }
        }
        stage('Build') {
            steps {
                echo 'Building the project...'
                sh 'dotnet build --configuration Release --no-restore'
            }
        }
        stage('Test') {
            steps {
                echo 'Running unit tests...'
                sh 'dotnet test --no-build --configuration Release --verbosity normal'
            }
        }
    }
    post {
        always {
            echo 'Cleaning workspace...'
            cleanWs() // Ensure the workspace is cleaned after each run
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed. Check logs for details.'
        }
    }
}
