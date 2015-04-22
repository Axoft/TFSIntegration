# TFSIntegration
TFSIntegration is a tool to handle TFS SOAP Alerts.

#Change log
*1.0.0:* Handle work items changes alerts. Current handler implementation is used to create items by some particular conditions.

# Configuration
1. Add an Alert ("A new work item is created in this team project") 
2. Set alert format to SOAP and set your host address in "Send to" field. This host addess has to be setted in service configuration to.
3. Open App.config and set <add key="host" value=""/> with the same host at previous point. IMPORTANT: This host needs to be accessible by TFS.
4. Open program.cs and set your credentials, or some new user that has rights to create tasks.
5. Create a configuration file in the service folder named `WorkItemChangedEventConfiguration.xml`
6. Start the service:

Configuration file template:
```
     <Configuration>
       <EventTypes>
         <EventType WorkItemType="XXX" PortfolioProject="YYY">
           <Tags>
             <Tag Value="">
             </Tag>
             <Tag Value="DEFAULT">
             </Tag>
             <Tag Value="ZZZ">
               <Items>
                 <Item>
                   <Title></Title>
                   <Tag></Tag>
                   <AssignTo></AssignTo>
                   <ItemType></ItemType>
                 </Item>
                 <Item>
                   <Title></Title>
                   <Tag></Tag>
                   <AssignTo></AssignTo>
                   <ItemType></ItemType>
                 </Item>
               </Items>
             </Tag>
           </Tags>
         </EventType>
       </EventTypes>
     </Configuration>
```
`<Tag Value="DEFAULT">` would be processed always.

`<Tag Value="">` would be processed if changed item does not have any tag.

`<Tag Value="ZZZ">` (Custom tags) are processed if matches with changed item tags.
