# TopJobs
A web application meant to connect employers and job candidates in the IT sector. Using a carefully designed algorithm, the app calculates the matching percentage between the applicant and the job ad. As a result, each candidate gets an ordered list of the open positions which best fit his or her skills and preferences, while each employer can easily check which applicants suit best the job requirements.

## Content
- [Functional requirements](#functional-requirements)
- [Goals](#goals)
- [Tech stack](#tech-stack)
- [Calculating match](#calculating-match)
  - [Criteria importance](#criteria-importance)
  - [Criteria fulfillment](#criteria-fulfillment)
- [Database model](#database)
- [Screenshots of the application](#screenshots-of-the-application)
  - [Candidate views](#candidate-views)
  - [Employer views](#employer-views)
  - [Admin views](#admin-views)
    
## Functional requirements
![Use-case diagram](https://github.com/DenforO/TopJobs_Web/blob/main/docs/UseCaseDiagram.png)

The users of the application are separated into three categories - ***candidates***, ***employers*** and ***admins***.

Common functionalities shared between *all groups* of users is editing their user data, seeing trends and getting recommendations in their respective home pages. 

In addition, *employers* are able to verify employees' experience in their company, upload new job ads and see who has applied for them. The list of candidates should be ordered by matching percentage, calculated by a position-level-specific algorithm. 

That same algorithm provides an ordered list of job ads to the *applicants*, who also get notified via e-mail if their application has been approved by the employer (and therefore should prepare for an upcoming interview). 

Some *admin*-specific features include managing user roles, registering new companies and employers. The last two task are exclusive to the admins in order to avoid fake job ads.

 ## Goals

 The main goal of the web application is to provide the following:
 
 ![Goals](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Goals.png)
 
 - **Intuitive UI** - since the web application is aimed at candidates and employers in the IT sector, the technological literacy of the users is expected to be above average. Despite that, an intuitive and specific design is considered a key feature which could set the app apart from alternatives.
 - **Extensive info** - the users should be able to input as much information about their preferences and expectations, so each party could make an informed choice when either applying for a job opening or approving a candidate's application.
 - **Efficient search** - the system should quickly provide relevant results, based on the matching percentage between job ads and applicants.
 - **Profile as CV** - the profile of the applicants should contain the information and be structured as a CV. This saves time both for the applicants and employers. Once the candidate enters their data, their CV is designed automatically. Its standardized format also makes it easier for recruiters to go through applicants.
 - **Trend charts** - a feature to show tech stack related trends among uploaded job ads gives valuable information to candidates.
 - **E-mail notification** - applicants should not have to constantly check the site in order to be informed about their applications' status. Instead an e-mail notification should be sent when a candidate has been approved and should expect a call/interview from the employer.
 - **Security** - the system should provide secure ways of storing sensitive data and also prevent potential frauds.

 ## Tech stack
 The web application has been realized with the following technologies:
 - ASP.NET Core MVC
 - Entity Framework Core
 - SQL Server
 - React.NET (for trend charts)

## Calculating match
A key feature of the web application is its match calculating algorithm. It helps candidates in finding relevant job ads and employers in checking which applicants are best suited for their job ad. 

The match between a candidate and a job ad is based on ***8 criteria***:
![Match criteria](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchCriteria.png)

### Criteria importance
Each criterion is of different importance depending on the level of the position which the candidate is applying for- Intern, Junior, Mid or Senior.

- **Intern**
  - positions are usually aimed at students, seeking opportunities for flexible part time jobs
  - most focus is given to the *working hours*, *flexible schedule* and *work from home* criteria
  - The specifics of the position and the known technologies are considered of low importance here, since most interns are just starting out their career paths and positions do not require much previous experience
  
  ![Pie chart showing each criterion's importance for Interns](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchIntern.png)

- **Junior**
  - Although higher level than internships, these job openings are also often targeting students or freshly graduated candidates
  - Balance between *flexibility* (for students) and *job-specific knowledge* (at least some experience with the technologies)
  
  ![Pie chart showing each criterion's importance for Interns](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchJunior.png)
    
- **Mid**
  - Also covers job ads where the level has not been specified
  - The focus shifts towards *experience*, *known technologies* and specifics of the *position*
  - Criteria like the *working hours* are of less importance since most Mid level employees work full-time
  
  ![Pie chart showing each criterion's importance for Interns](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchMid.png)

- **Senior**
  - Employees are less likely to seek a completely different positions than what they have already specialized in
  - Employers who recruit seniors need experienced candidates with extensive specific knowledge
  - *Position*, *work experience* and *known technologies* make up for almost two thirds of the end result
  
  ![Pie chart showing each criterion's importance for Interns](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchSenior.png)

### Criteria fulfillment
Each criterion's fulfillment varies between 0% and 100%:

- **Position** - Considered 100% fulfilled when both the type and the level of the job, specified in the ad, are the same as the candidate's preference. When only the level is mismatched, the fulfillment is 70%. If only the level is the same, while the job type is different, the match is 30%. If neither of the two components matches, a 0% match is applied to this criterion.
  
  ![Table with examples for position match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_position.png)

- **Tech stack** - The number of common technologies between the candidate's preferences and the job opening's requirement is divided by the number of all listed technologies in the ad.
  
  ![Table with examples for tech stack match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_techStack.png)

- **Work experience** - The total months of the candidate's work experience are rounded to years. If the result is less than the required experience for the job opening, this criterion is considered 0% fulfilled. If it's equal or slightly more than the requirement, a 100% fulfillment is applied. Fulfillment degrades as the difference between the candidate's and the job ad's experience grows, since in such cases the candidate is considered overqualified.

  ![Table with examples for work experience match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_experience.png)
- **Education** - This criterion is fulfilled by more education entries by the applicant. In case of university, more points are given when the education has been completed rather than ongoing.

  ![Table with examples for education match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_education.png)

- **Location** - If the candidate has set their city (optional) and it is the same as the job opening's location, this criterion is 100% fulfilled. Same goes for different cities when work from home is allowed. 50% fulfillment is applied when the applicant's city has not been specified. 0% fulfillment when the cities don't match and work from home is not allowed.

  ![Table with examples for location match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_location.png)

- **Work from home** - 0% fulfillment when the candidate prefers to work from home while the job ad does not offer it. 100% fulfillment in all other cases.

  ![Table with examples for work from home match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_WFH.png)

- **Flexible schedule** - 0% fulfillment when the candidate would rather have a flexible schedule while the job ad does not offer it. 100% fulfillment in all other cases.

  ![Table with examples for flexible schedule match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_flexible.png)

- **Working hours** - 1 - Equals | (candidate's preferred working hours) - (job ad's working hours) | * 0.25

  ![Table with examples for working hours match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/MatchExample_workingHours.png)

## Database
For the execution of the described algorithm and the implementation of the functional requirements, the following database model has been designed in SQL Server:

![Table with examples for working hours match](https://github.com/DenforO/TopJobs_Web/blob/main/docs/TopJobs_DB_2.drawio.png)

## Screenshots of the application
Some of the main functionalities are showcased in the following screenshots of the web application:

### Candidate views

- User profile (serving as their CV)

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Screenshot_userProfile.png)

- Trends of chosen technologies during each month

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/trend1.png)

- Top 6 Most popular technologies at the moment

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/trend2.png)

- User preferences page

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Screenshot_userPreferences.png)

- Job ads (ordered by match percentage)

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Screenshot_jobAds.png)

### Employer views

- Employer's job ads (ordered by date of upload, without archived)

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Screenshot_myJobAds.png)

- Job ad candidates (ordered by match percentage)

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Screenshot_candidates.png)
  
### Admin views
- Admin panel

  ![Screenshot of the user profile in a CV format](https://github.com/DenforO/TopJobs_Web/blob/main/docs/Screenshot_admin.png)
