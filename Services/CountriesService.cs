using Entity;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    //private field
    private readonly PesonsDbContext _pesonsDbContext;

    //constructor
    public CountriesService(PesonsDbContext pesonsDbContext)
    {
       
        _pesonsDbContext = pesonsDbContext;
    }

    public async Task<CountryResponse> AddCountryAsync(CountryAddRequest? countryAddRequest)
    {

        //Validation: countryAddRequest parameter can't be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        //Validation: CountryName can't be null
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        //Validation: CountryName can't be duplicate
        if (_pesonsDbContext.Countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
        {
            throw new ArgumentException("Given country name already exists");
        }

        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        //generate CountryID
        country.CountryID = Guid.NewGuid();

        //Add country object into _countries
        _pesonsDbContext.Countries.Add(country);

        await _pesonsDbContext.SaveChangesAsync();

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _pesonsDbContext.Countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
            return null;

        Country? country_response_from_list = _pesonsDbContext.Countries.FirstOrDefault(temp => temp.CountryID == countryID);

        if (country_response_from_list == null)
            return null;

        return country_response_from_list.ToCountryResponse();
    }

}




